using Eshop.Module.Pricing.Models;
using Shared.DDD;

namespace Eshop.Module.Pricing.Models.Events;

public sealed record ExchangeRateCreatedEvent(
    Guid EventId,
    DateTimeOffset OccurredOnUtc,
    ExchangeRate ExchangeRate
) : DomainEvent(EventId, OccurredOnUtc)
{
    public static ExchangeRateCreatedEvent Create(ExchangeRate exchangeRate)
    {
        return new ExchangeRateCreatedEvent(
            Guid.NewGuid(),
            DateTimeOffset.UtcNow,
            exchangeRate
        );
    }
}