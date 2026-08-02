using Shared.DDD;

public class Checkout : EntityBase<Guid>, IAuditableEntity
{
    public Guid BasketId { get; private set; }
    public Guid UserId { get; private set; }
    public string Status { get; private set; }
    public decimal TotalAmount { get; private set; }



    public DateTimeOffset? LatestUpdatedOn { get; private set; }

    public string LatestUpdatedById { get; private set; }

    public DateTimeOffset CreatedOn { get; private set; }

    public string CreatedById { get; private set; }

    public static Checkout Create(Guid basketId, Guid userId, decimal totalAmount)
    {
        return new Checkout
        {
            Id = Guid.NewGuid(),
            BasketId = basketId,
            UserId = userId,
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