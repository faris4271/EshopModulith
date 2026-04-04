using Shared.Eventing;
using Shared.Message.Events;

namespace Shared.Message.OutBox
{
    public interface IOutBoxStore
    {
        Task AddAsync(IntegrationEvent @event, CancellationToken ct = default);

        Task<IReadOnlyList<OutboxMessage>> GetPendingBatchAsync(int batchSize, CancellationToken ct = default);

        Task MarkAsProcessedAsync(OutboxMessage message, CancellationToken ct = default);

        Task MarkAsFailedAsync(OutboxMessage message, string error, bool isDead, CancellationToken ct = default);
    }
}
