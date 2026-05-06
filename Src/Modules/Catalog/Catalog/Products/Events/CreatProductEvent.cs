using Catalog.Products.Models;
using Shared.DDD;

namespace Catalog.Products.Events
{
    public record CreatProductEvent(
        Guid EventId, DateTimeOffset OccurredOnUtc,
        Product Product 

        ) : DomainEvent(EventId,OccurredOnUtc)
    {
       public static CreatProductEvent Creat(Product product)
        {
            return new CreatProductEvent(
                Guid.NewGuid(),
                DateTimeOffset.UtcNow,
                product

                );

        }
    }
}
