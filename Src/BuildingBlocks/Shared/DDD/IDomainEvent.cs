using MediatR;

namespace Shared.DDD
{
    public interface IDomainEvent : INotification
    {
        Guid EventId { get; }

        /// <summary>
        /// Gets the UTC timestamp when the event occurred.
        /// </summary>
        DateTimeOffset OccurredOnUtc { get; }

        /// <summary>
        /// Gets the correlation identifier for tracing across boundaries.
        /// </summary>
        string? CorrelationId { get; }

        /// <summary>
        /// Gets the tenant identifier associated with the event.
        /// </summary>


    }

}
