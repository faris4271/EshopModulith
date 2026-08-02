using Shared.DDD;

public class Payment : EntityBase<Guid>, IAuditableEntity
{
    public Guid CheckoutId { get; private set; }
    public string PaymentMethod { get; private set; }
    public string PaymentStatus { get; private set; }
    public decimal Amount { get; private set; }

    public DateTimeOffset CreatedOn { get; private set; }

    public string CreatedById { get; private set; }

    public DateTimeOffset? LatestUpdatedOn { get; private set; }

    public string LatestUpdatedById { get; private set; }

    public static Payment Create(Guid checkoutId, string paymentMethod, decimal amount)
    {
        return new Payment
        {
            Id = Guid.NewGuid(),
            CheckoutId = checkoutId,
            PaymentMethod = paymentMethod,
            PaymentStatus = "Pending",
            Amount = amount,
            CreatedOn = DateTime.UtcNow
        };
    }

    public void UpdateStatus(string status)
    {
        PaymentStatus = status;
        LatestUpdatedOn = DateTime.UtcNow;
    }
}