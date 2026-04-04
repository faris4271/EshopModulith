using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Shared.Eventing.Contract;
using Shared.Message.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Eventing.RabbitMq
{
    internal class RabbitMqEventBus : IEventBus, IAsyncDisposable
    {
        private readonly IEventSerializer _eventSerializer;
        private readonly ILogger<RabbitMqEventBus> _logger;
        private readonly RabbitMqOptions _options;
        private readonly SemaphoreSlim _connectionLock = new(1, 1);
        private IConnection? _connection;
        private IChannel? _channel;
        private bool _disposed;

        public RabbitMqEventBus(IEventSerializer eventSerializer,
            ILogger<RabbitMqEventBus> logger, RabbitMqOptions options,
            SemaphoreSlim connectionLock, IConnection?
            connection, IChannel? channel, bool disposed)
        {
            _eventSerializer = eventSerializer;
            _logger = logger;
            _options = options;
            _connectionLock = connectionLock;
            _connection = connection;
            _channel = channel;
            _disposed = disposed;
        }

        public ValueTask DisposeAsync()
        {
            throw new NotImplementedException();
        }

        public Task PublishAsync(IntegrationEvent @event, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public async Task PublishAsync(IEnumerable<IntegrationEvent> events, CancellationToken ct = default)
        {
            foreach (IntegrationEvent @event in events)
            {
                await PublishSingleAsync(@event, ct);
            }
        }

        private async Task PublishSingleAsync(IntegrationEvent @event, CancellationToken ct)
        {
            var eventType = @event.GetType();
            var routingKey = eventType.FullName ?? eventType.Name;

            var payload = _eventSerializer.Serialize(@event);

            var body = Encoding.UTF8.GetBytes(payload);

            var retryCount = 0;
            var maxRetry = 0;
            var retryDelay = TimeSpan.FromMilliseconds(_options.PublishRetryDelayMs);

            while (true)
            {
                try
                {
                    if (_channel == null)
                        throw new InvalidOperationException("RabbitMq chanel not initialized");

                    var properties = new BasicProperties
                    {
                        MessageId = @event.Id.ToString(),
                      
                    };

                }
                catch (Exception)
                {

                    throw;
                }
            }


        }
    }
}