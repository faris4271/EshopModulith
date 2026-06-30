using Shared.Eventing.Contract;

namespace Shared.Eventing.OutBox
{
    public interface IOutBoxStore
    {
        Task AddAsync(IIntegrationEvent @event, CancellationToken ct = default);

        Task<IReadOnlyList<OutboxMessage>> GetPendingBatchAsync(int batchSize, CancellationToken ct = default);

        Task MarkAsProcessedAsync(OutboxMessage message, CancellationToken ct = default);

        Task MarkAsFailedAsync(OutboxMessage message, string error, bool isDead, CancellationToken ct = default);
    }
}
