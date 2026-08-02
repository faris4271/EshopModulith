using Eshop.Module.Checkout.Entities.Events;
using Shared.DDD;
using System.Collections.Generic;

namespace Eshop.Module.Checkout.Entities;

public sealed class Checkout : EntityBase<Guid>, IAuditableEntity
{
    // Domain properties
    public Guid UserId { get; private set; } // mapped from CustomerId
    public Money TotalAmount { get; private set; }
    public string Status { get; private set; } = "Pending";

    public string? CouponCode { get; private set; }
    public string? CouponRuleName { get; private set; }
    public string? ShippingMethod { get; private set; }
    public bool IsProductPriceIncludeTax { get; private set; }
    public Money? ShippingAmount { get; private set; }
    public Money? TaxAmount { get; private set; }
    public Guid? VendorId { get; private set; }
    public string? ShippingData { get; private set; } // JSON serialized
    public string? OrderNote { get; private set; }

    private readonly List<CheckoutItem> _checkoutItems = [];
    public IReadOnlyCollection<CheckoutItem> CheckoutItems => _checkoutItems.AsReadOnly();

    // IAuditableEntity implementation (handled by AuditableEntityInterceptor)
    public DateTimeOffset CreatedOn { get; private set; }
    public string? CreatedById { get; private set; }
    public DateTimeOffset? LatestUpdatedOn { get; private set; }
    public string? LatestUpdatedById { get; private set; }

    private Checkout() { }

    public static Checkout Create(Guid userId, bool isProductPriceIncludeTax, string? couponCode = null, string? shippingMethod = null)
    {


        var checkout = new Checkout
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            IsProductPriceIncludeTax = isProductPriceIncludeTax,
            CouponCode = couponCode,
            ShippingMethod = shippingMethod,
            CreatedOn = DateTimeOffset.UtcNow
        };

        checkout.AddDomainEvent(new CheckoutCreatedEvent(Guid.NewGuid(), checkout.Id));
        return checkout;
    }

    public void AddItem(Guid productId, int quantity)
    {
        var item = CheckoutItem.Create(productId, quantity, Id);
        _checkoutItems.Add(item);
    }

    public void MarkAsCompleted()
    {
        Status = "Completed";
        AddDomainEvent(new CheckoutUpdatedEvent(Guid.NewGuid(), Id));
    }

    public void AddAmuntAndShippingMethud(string shippingMethud, Money amunt)
    {
        ArgumentNullException.ThrowIfNull(shippingMethud, nameof(shippingMethud));
        ArgumentNullException.ThrowIfNull(amunt, nameof(amunt));
        this.ShippingMethod = shippingMethud;
        this.ShippingAmount = amunt;
    }

    public void UpdateTaxAmount(Money money)
    {
        ArgumentNullException.ThrowIfNull(money, nameof(money));
        this.TaxAmount = money;

    }

}
