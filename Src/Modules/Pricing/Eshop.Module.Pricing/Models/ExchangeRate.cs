using Eshop.Module.Pricing.Models.Events;
using Shared.DDD;

namespace Eshop.Module.Pricing.Models;

public sealed class ExchangeRate : EntityBase<Guid>, IAuditableEntity, ISoftDelete
{
    public string FromCurrency { get; private set; } = default!;
    public string ToCurrency { get; private set; } = default!;
    public decimal Rate { get; private set; }
    public DateTime LastUpdatedUtc { get; private set; }

    public DateTimeOffset CreatedOn { get; private set; }

    public string? CreatedById { get; private set; }

    public DateTimeOffset? LatestUpdatedOn { get; private set; }

    public string? LatestUpdatedById { get; private set; }

    public bool IsDeleted { get; private set; }

    public DateTimeOffset? DeletedOnUtc { get; private set; }

    public string? DeletedBy { get; private set; }

    // Private constructor for EF Core
    private ExchangeRate() { }

    // Factory method - the only way to create
    public static ExchangeRate Create(string fromCurrency, string toCurrency, decimal rate)
    {
        ArgumentNullException.ThrowIfNull(fromCurrency);
        ArgumentNullException.ThrowIfNull(toCurrency);

        var entity = new ExchangeRate
        {
            Id = Guid.CreateVersion7(),
            FromCurrency = fromCurrency.ToUpperInvariant(),
            ToCurrency = toCurrency.ToUpperInvariant(),
            Rate = rate,
            LastUpdatedUtc = DateTime.UtcNow
        };

        entity.AddDomainEvent(ExchangeRateCreatedEvent.Create(entity));
        return entity;
    }

    // Domain methods for state changes
    public void UpdateRate(decimal newRate)
    {
        Rate = newRate;
        LastUpdatedUtc = DateTime.UtcNow;

        AddDomainEvent(ExchangeRateUpdatedEvent.Create(this, new[] { nameof(Rate), nameof(LastUpdatedUtc) }));
    }

    public void Delete()
    {
        IsDeleted = true;
        AddDomainEvent(ExchangeRateDeletedEvent.Create(Id));
    }
}