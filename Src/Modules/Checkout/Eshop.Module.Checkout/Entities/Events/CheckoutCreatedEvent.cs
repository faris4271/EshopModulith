using Shared.DDD;


namespace Eshop.Module.Checkout.Entities.Events;

public sealed record CheckoutCreatedEvent(Guid EventId, Guid CheckoutId, DateTimeOffset OccurredOnUtc)
    : DomainEvent(EventId, OccurredOnUtc)
{
    public CheckoutCreatedEvent(Guid eventId, Guid checkoutId)
        : this(eventId, checkoutId, DateTimeOffset.UtcNow) { }
}
