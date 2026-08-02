using Shared.DDD;

public class Order : EntityBase<Guid>, IAuditableEntity
{
    public Guid CheckoutId { get; private set; }
    public string OrderNumber { get; private set; }
    public string Status { get; private set; }
    public decimal TotalAmount { get; private set; }

    public DateTimeOffset CreatedOn { get; private set; }

    public string CreatedById { get; private set; }

    public DateTimeOffset? LatestUpdatedOn { get; private set; }

    public string LatestUpdatedById { get; private set; }

    public static Order Create(Guid checkoutId, string orderNumber, decimal totalAmount)
    {
        return new Order
        {
            Id = Guid.NewGuid(),
            CheckoutId = checkoutId,
            OrderNumber = orderNumber,
            Status = "Pending",
            TotalAmount = totalAmount,
            CreatedOn = DateTime.UtcNow
        };
    }

    public void UpdateStatus(string status)
    {
        Status = status;
        LatestUpdatedOn = DateTime.UtcNow;
    }
}