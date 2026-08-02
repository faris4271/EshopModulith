using Shared.DDD;

namespace Eshop.Module.Pricing.Models.Events;

public sealed record ExchangeRateDeletedEvent(
    Guid EventId,
    DateTimeOffset OccurredOnUtc,
    Guid ExchangeRateId
) : DomainEvent(EventId, OccurredOnUtc)
{
    public static ExchangeRateDeletedEvent Create(Guid exchangeRateId)
    {
        return new ExchangeRateDeletedEvent(
            Guid.NewGuid(),
            DateTimeOffset.UtcNow,
            exchangeRateId
        );
    }
}