using Shared.DDD;
using System;

namespace Eshop.Module.Shipping.Models;

public record ShippingProviderCreatedEvent(
    Guid EventId, DateTimeOffset OccurredOnUtc,
    ShippingProvider ShippingProvider) : DomainEvent(EventId, OccurredOnUtc)
{
    public static ShippingProviderCreatedEvent Create(ShippingProvider shippingProvider)
    {
        return new ShippingProviderCreatedEvent(
            Guid.NewGuid(),
            DateTimeOffset.UtcNow,
            shippingProvider);
    }
}

public record ShippingProviderUpdatedEvent(
    Guid EventId, DateTimeOffset OccurredOnUtc,
    ShippingProvider ShippingProvider) : DomainEvent(EventId, OccurredOnUtc)
{
    public static ShippingProviderUpdatedEvent Create(ShippingProvider shippingProvider)
    {
        return new ShippingProviderUpdatedEvent(
            Guid.NewGuid(),
            DateTimeOffset.UtcNow,
            shippingProvider);
    }
}

public record ShippingProviderStatusChangedEvent(
    Guid EventId, DateTimeOffset OccurredOnUtc,
    ShippingProvider ShippingProvider, bool IsEnabled) : DomainEvent(EventId, OccurredOnUtc)
{
    public static ShippingProviderStatusChangedEvent Create(ShippingProvider shippingProvider, bool isEnabled)
    {
        return new ShippingProviderStatusChangedEvent(
            Guid.NewGuid(),
            DateTimeOffset.UtcNow,
            shippingProvider,
            isEnabled);
    }
}

public record ShippingProviderDeletedEvent(
    Guid EventId, DateTimeOffset OccurredOnUtc,
    ShippingProvider ShippingProvider) : DomainEvent(EventId, OccurredOnUtc)
{
    public static ShippingProviderDeletedEvent Create(ShippingProvider shippingProvider)
    {
        return new ShippingProviderDeletedEvent(
            Guid.NewGuid(),
            DateTimeOffset.UtcNow,
            shippingProvider);
    }
}