using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Eventing.Contract;

namespace Shared.Eventing.OutBox
{
    internal class EfCoreOutboxStore<TDbContext>(
        TDbContext _dbContext, IEventSerializer _eventSerializer,
        ILogger<EfCoreOutboxStore<TDbContext>> _logger

        ) : IOutBoxStore where TDbContext : DbContext
    {
        public async Task AddAsync(IIntegrationEvent @event, CancellationToken ct = default)
        {
            var payload = _eventSerializer.Serialize(@event);

            var message = new OutboxMessage
            {
                Id = @event.Id,
                CreatedOnUtc = @event.OccurredOnUtc,
                Type = @event.GetType().AssemblyQualifiedName ?? @event.GetType().FullName!,
                Payload = payload,
                CorrelationId = @event.CorrelationId,
                RetryCount = 0,
                IsDead = false
            };

            await _dbContext.Set<OutboxMessage>().AddAsync(message, ct);
            await _dbContext.SaveChangesAsync(ct).ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<OutboxMessage>> GetPendingBatchAsync(int batchSize, CancellationToken ct = default)
        {
            return await _dbContext.Set<OutboxMessage>().
                Where(x => x.ProcessedOnUtc == null && !x.IsDead).
                OrderBy(c => c.CreatedOnUtc)
                .Take(batchSize).ToListAsync(ct)
                .ConfigureAwait(false);



        }

        public async Task MarkAsFailedAsync(OutboxMessage message, string error, bool isDead, CancellationToken ct = default)
        {

            message.IsDead = isDead;

            message.RetryCount++;

            message.LastError = error;


            _logger.LogWarning("Outbox message {MessageId} failed. RetryCount={RetryCount}, IsDead={IsDead}, Error={Error}",
                   message.Id, message.RetryCount, message.IsDead, error);

            _dbContext.Set<OutboxMessage>().Update(message);

            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

        }

        public async Task MarkAsProcessedAsync(OutboxMessage message, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(message);

            message.ProcessedOnUtc = DateTime.UtcNow;

            _dbContext.Update(message);

            await _dbContext.SaveChangesAsync(ct).ConfigureAwait(false);

        }
    }
}
