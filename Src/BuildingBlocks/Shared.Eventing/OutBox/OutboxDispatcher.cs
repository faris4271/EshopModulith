using MassTransit; // إضافة مكتبة MassTransit
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Eventing;
using Shared.Eventing.Contract;
using Shared.Message.OutBox;

public sealed class OutboxDispatcher
{
    private readonly IOutBoxStore _outbox;
    // استبدال IEventBus بـ IPublishEndpoint التابع لـ MassTransit
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IEventSerializer _serializer;
    private readonly ILogger<OutboxDispatcher> _logger;
    private readonly EventingOptions _options;

    public OutboxDispatcher(
        IOutBoxStore outbox,
        IPublishEndpoint publishEndpoint, // حقن MassTransit
        IEventSerializer serializer,
        IOptions<EventingOptions> options,
        ILogger<OutboxDispatcher> logger)
    {
        _outbox = outbox;
        _publishEndpoint = publishEndpoint;
        _serializer = serializer;
        _logger = logger;
        _options = options.Value;
    }

    public async Task DispatchAsync(CancellationToken ct = default)
    {
        var batchSize = _options.OutboxBatchSize <= 0 ? 100 : _options.OutboxBatchSize;
        var messages = await _outbox.GetPendingBatchAsync(batchSize, ct).ConfigureAwait(false);

        if (messages.Count == 0) return;

        foreach (var message in messages)
        {
            try
            {

                var @event = _serializer.Deserialize(message.Payload, message.Type);
                if (@event is null)
                {
                    await _outbox.MarkAsFailedAsync(message, "Cannot deserialize", isDead: true, ct).ConfigureAwait(false);
                    continue;
                }


                await _publishEndpoint.Publish(@event, ct).ConfigureAwait(false);


                await _outbox.MarkAsProcessedAsync(message, ct).ConfigureAwait(false);
            }
            catch (Exception ex)
            {

                var isDead = message.RetryCount + 1 >= (_options.OutboxMaxRetries <= 0 ? 5 : _options.OutboxMaxRetries);
                await _outbox.MarkAsFailedAsync(message, ex.Message, isDead, ct).ConfigureAwait(false);
            }
        }
    }
}