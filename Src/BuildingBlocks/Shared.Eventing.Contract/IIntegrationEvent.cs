namespace Shared.Eventing.Contract
{
    public interface IIntegrationEvent
    {
        Guid Id { get; }

        DateTime OccurredOnUtc { get; }

        /// <summary>
        /// Tenant identifier for tenant-scoped events. Null for global events.
        /// </summary>

        string CorrelationId { get; }

        /// <summary>
        /// Logical source of the event (e.g., module or service name).
        /// </summary>
        string Source { get; }
    }
}
