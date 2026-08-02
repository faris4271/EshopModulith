using Shared.DDD;

namespace Eshop.Module.Checkout.Entities;

public sealed class CheckoutItem : EntityBase<Guid>, IAuditableEntity
{
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public Guid CheckoutId { get; private set; }

    // IAuditableEntity implementation
    public DateTimeOffset CreatedOn { get; private set; }
    public string? CreatedById { get; private set; }
    public DateTimeOffset? LatestUpdatedOn { get; private set; }
    public string? LatestUpdatedById { get; private set; }

    private CheckoutItem() { }

    public static CheckoutItem Create(Guid productId, int quantity, Guid checkoutId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);

        return new CheckoutItem
        {
            Id = Guid.NewGuid(),
            ProductId = productId,
            Quantity = quantity,
            CheckoutId = checkoutId,
            CreatedOn = DateTimeOffset.UtcNow
        };
    }
}
