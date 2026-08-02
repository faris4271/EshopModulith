using Shared.DDD;

public class ShippingAddress : EntityBase<Guid>, IAuditableEntity
{
    public Guid CheckoutId { get; private set; }
    public string AddressLine1 { get; private set; }
    public string AddressLine2 { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public string PostalCode { get; private set; }
    public string Country { get; private set; }

    public DateTimeOffset CreatedOn { get; private set; }

    public string CreatedById { get; private set; }

    public DateTimeOffset? LatestUpdatedOn { get; private set; }

    public string LatestUpdatedById { get; private set; }

    public static ShippingAddress Create(Guid checkoutId, string addressLine1, string addressLine2, string city, string state, string postalCode, string country)
    {
        return new ShippingAddress
        {
            Id = Guid.CreateVersion7(),
            CheckoutId = checkoutId,
            AddressLine1 = addressLine1,
            AddressLine2 = addressLine2,
            City = city,
            State = state,
            PostalCode = postalCode,
            Country = country,
            CreatedOn = DateTime.UtcNow
        };
    }
}