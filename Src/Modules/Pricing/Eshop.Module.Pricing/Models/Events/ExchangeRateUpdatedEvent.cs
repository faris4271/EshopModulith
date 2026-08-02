using Eshop.Module.Pricing.Models;
using Shared.DDD;

namespace Eshop.Module.Pricing.Models.Events;

public sealed record ExchangeRateUpdatedEvent(
    Guid EventId,
    DateTimeOffset OccurredOnUtc,
    ExchangeRate ExchangeRate,
    string[] ChangedProperties
) : DomainEvent(EventId, OccurredOnUtc)
{
    public static ExchangeRateUpdatedEvent Create(ExchangeRate exchangeRate, string[] changedProperties)
    {
        return new ExchangeRateUpdatedEvent(
            Guid.NewGuid(),
            DateTimeOffset.UtcNow,
            exchangeRate,
            changedProperties
        );
    }
}