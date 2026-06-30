using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Eventing.Contract;
using Shared.Eventing.Inbox;
using System.Reflection;
using System.Text;

namespace Shared.Eventing.RabbitMq;

/// <summary>
/// Background service that consumes integration events from RabbitMQ
/// and dispatches them to registered IIntegrationEventHandler implementations.
/// </summary>
public sealed class RabbitMqConsumerHostedService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly RabbitMqOptions _options;
    private readonly IEventSerializer _serializer;
    private readonly ILogger<RabbitMqConsumerHostedService> _logger;
    private readonly Assembly[] _handlerAssemblies;

    private IConnection? _connection;
    private IChannel? _channel;

    public RabbitMqConsumerHostedService(
        IServiceScopeFactory scopeFactory,
        IOptions<RabbitMqOptions> options,
        IEventSerializer serializer,
        ILogger<RabbitMqConsumerHostedService> logger,
        IEnumerable<Assembly> handlerAssemblies)
    {
        _scopeFactory = scopeFactory;
        _options = options.Value;
        _serializer = serializer;
        _logger = logger;
        _handlerAssemblies = handlerAssemblies.ToArray();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("RabbitMQ Consumer Hosted Service starting...");

        try
        {
            await ConnectAsync(stoppingToken);
            await DeclareExchangeAsync(stoppingToken);
            await SubscribeHandlersAsync(stoppingToken);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Failed to start RabbitMQ consumer. The service will stop.");
            throw;
        }

        // Keep the service running until cancelled
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private async Task ConnectAsync(CancellationToken ct)
    {
        var factory = new ConnectionFactory
        {
            HostName = _options.Host,
            Port = _options.Port,
            UserName = _options.UserName,
            Password = _options.Password,
            VirtualHost = _options.VirtualHost
        };

        if (_options.UseSsl)
        {
            factory.Ssl = new SslOption { Enabled = true };
        }

        _connection = await factory.CreateConnectionAsync(ct);
        _channel = await _connection.CreateChannelAsync(cancellationToken: ct);

        _logger.LogInformation(
            "RabbitMQ consumer connected to {Host}:{Port}/{VirtualHost}",
            _options.Host, _options.Port, _options.VirtualHost);
    }

    private async Task DeclareExchangeAsync(CancellationToken ct)
    {
        await _channel!.ExchangeDeclareAsync(
            exchange: _options.ExchangeName,
            type: ExchangeType.Topic,
            durable: true,
            autoDelete: false,
            cancellationToken: ct);

        _logger.LogInformation("Exchange {Exchange} declared.", _options.ExchangeName);
    }

    private async Task SubscribeHandlersAsync(CancellationToken stoppingToken)
    {
        var handlerMappings = DiscoverHandlers();

        if (handlerMappings.Count == 0)
        {
            _logger.LogWarning("No integration event handlers found. No queues will be created.");
            return;
        }

        foreach (var mapping in handlerMappings)
        {
            var routingKey = mapping.EventType.FullName ?? mapping.EventType.Name;
            var queueName = $"{_options.QueuePrefix}.{mapping.HandlerType.Name}.{routingKey}";

            await _channel!.QueueDeclareAsync(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                cancellationToken: stoppingToken);

            await _channel!.QueueBindAsync(
                queue: queueName,
                exchange: _options.ExchangeName,
                routingKey: routingKey,
                cancellationToken: stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += (model, ea) => HandleMessageAsync(ea, mapping, stoppingToken);

            await _channel!.BasicConsumeAsync(
                queue: queueName,
                autoAck: false,
                consumer: consumer,
                cancellationToken: stoppingToken);

            _logger.LogInformation(
                "Subscribed to queue {Queue} with routing key {RoutingKey} for handler {Handler}",
                queueName, routingKey, mapping.HandlerType.Name);
        }
    }

    private async Task HandleMessageAsync(
        BasicDeliverEventArgs ea,
        HandlerMapping mapping,
        CancellationToken ct)
    {
        var handlerName = mapping.HandlerType.FullName ?? mapping.HandlerType.Name;
        var eventId = Guid.Empty;

        try
        {
            // Extract event ID from MessageId
            if (ea.BasicProperties.MessageId != null)
            {
                Guid.TryParse(ea.BasicProperties.MessageId, out eventId);
            }

            // Get event type name from header (RabbitMQ stores headers as byte[])
            var body = ea.Body.ToArray();
            string? eventTypeName = null;

            if (ea.BasicProperties.Headers != null &&
                ea.BasicProperties.Headers.TryGetValue("event-type", out var headerValue) &&
                headerValue is byte[] headerBytes)
            {
                eventTypeName = Encoding.UTF8.GetString(headerBytes);
            }

            // Fallback: use the AppDomain type from the mapping
            eventTypeName ??= mapping.EventType.AssemblyQualifiedName ?? mapping.EventType.FullName!;

            if (string.IsNullOrEmpty(eventTypeName))
            {
                _logger.LogWarning(
                    "Received message without a valid event-type header on queue. Rejecting message.");
                await _channel!.BasicRejectAsync(ea.DeliveryTag, requeue: false);
                return;
            }

            _logger.LogDebug(
                "Received message. EventId={EventId}, EventType={EventType}, Handler={Handler}",
                eventId, eventTypeName, handlerName);

            // Deserialize
            var @event = _serializer.Deserialize(Encoding.UTF8.GetString(body), eventTypeName);
            if (@event == null)
            {
                _logger.LogWarning(
                    "Failed to deserialize event of type {EventType}. Rejecting message.",
                    eventTypeName);
                await _channel!.BasicRejectAsync(ea.DeliveryTag, requeue: false);
                return;
            }

            // Create a DI scope for handler resolution
            using var scope = _scopeFactory.CreateScope();
            var provider = scope.ServiceProvider;

            // Idempotency check via Inbox
            var inbox = provider.GetService<IInboxStore>();
            if (inbox != null && eventId != Guid.Empty)
            {
                if (await inbox.HasProcessedAsync(eventId, handlerName, ct))
                {
                    _logger.LogDebug(
                        "Skipping already processed event {EventId} for handler {Handler}",
                        eventId, handlerName);
                    await _channel!.BasicAckAsync(ea.DeliveryTag, multiple: false);
                    return;
                }
            }

            // Resolve handler by INTERFACE type (not concrete type)
            var handlerInstance = provider.GetService(mapping.InterfaceType);
            if (handlerInstance == null)
            {
                _logger.LogWarning(
                    "No handler registered for {HandlerInterface}. EventId={EventId}. Acking to remove from queue.",
                    mapping.InterfaceType.Name, eventId);
                await _channel!.BasicAckAsync(ea.DeliveryTag, multiple: false);
                return;
            }

            var method = mapping.InterfaceType.GetMethod("HandleAsync");
            if (method == null)
            {
                _logger.LogError(
                    "Handler {Handler} does not implement HandleAsync. Rejecting message.",
                    handlerName);
                await _channel!.BasicRejectAsync(ea.DeliveryTag, requeue: false);
                return;
            }

            // Invoke handler
            var task = (Task)method.Invoke(handlerInstance, new object[] { @event, ct })!;
            await task;

            // Mark as processed in inbox
            if (inbox != null && eventId != Guid.Empty)
            {
                var typeName = mapping.EventType.AssemblyQualifiedName ?? mapping.EventType.FullName!;
                await inbox.MarkProcessedAsync(eventId, handlerName, typeName, ct);
            }

            // Acknowledge success
            await _channel!.BasicAckAsync(ea.DeliveryTag, multiple: false);

            _logger.LogDebug(
                "Successfully handled event {EventId} with handler {Handler}",
                eventId, handlerName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error handling event {EventId} with handler {Handler}. Nacking for requeue.",
                eventId, handlerName);

            try
            {
                await _channel!.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true);
            }
            catch (Exception nackEx)
            {
                _logger.LogError(nackEx, "Failed to Nack message after handler error.");
            }
        }
    }

    private List<HandlerMapping> DiscoverHandlers()
    {
        var mappings = new List<HandlerMapping>();

        foreach (var assembly in _handlerAssemblies)
        {
            try
            {
                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    if (type.IsAbstract || type.IsInterface) continue;

                    var interfaces = type.GetInterfaces()
                        .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IIntegrationEventHandler<>));

                    foreach (var iface in interfaces)
                    {
                        mappings.Add(new HandlerMapping
                        {
                            HandlerType = type,
                            InterfaceType = iface,
                            EventType = iface.GetGenericArguments()[0]
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to scan assembly {Assembly} for handlers.", assembly.FullName);
            }
        }

        _logger.LogInformation("Discovered {Count} integration event handler mappings.", mappings.Count);
        return mappings;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("RabbitMQ Consumer Hosted Service stopping...");

        if (_channel is { IsOpen: true })
        {
            try { await _channel.CloseAsync(); }
            catch { /* ignore */ }
            _channel.Dispose();
            _channel = null;
        }

        if (_connection is { IsOpen: true })
        {
            try { await _connection.CloseAsync(); }
            catch { /* ignore */ }
            _connection.Dispose();
            _connection = null;
        }

        await base.StopAsync(cancellationToken);
    }

    private sealed class HandlerMapping
    {
        public required Type HandlerType { get; init; }
        public required Type InterfaceType { get; init; }
        public required Type EventType { get; init; }
    }
}