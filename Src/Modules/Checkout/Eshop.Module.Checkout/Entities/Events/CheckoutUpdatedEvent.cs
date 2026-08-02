using Shared.DDD;

namespace Eshop.Module.Checkout.Entities.Events;

public sealed record CheckoutUpdatedEvent(Guid EventId, Guid CheckoutId, DateTimeOffset OccurredOnUtc)
    : DomainEvent(EventId, OccurredOnUtc)
{
    public CheckoutUpdatedEvent(Guid eventId, Guid checkoutId)
        : this(eventId, checkoutId, DateTimeOffset.UtcNow) { }
}
