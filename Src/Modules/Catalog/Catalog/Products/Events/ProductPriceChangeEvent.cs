using Catalog.Products.Models;
using Shared.DDD;

namespace Catalog.Products.Events
{
    public record ProductPriceChangeEvent : DomainEvent
    {
        public ProductPriceChangeEvent(Guid EventId, DateTimeOffset OccurredOnUtc,Product product, string? CorrelationId = null) : base(EventId, OccurredOnUtc, CorrelationId)
        {

        }

        public static ProductPriceChangeEvent Create(Product product)
        {
            return new ProductPriceChangeEvent(Guid.NewGuid(), DateTimeOffset.UtcNow, product);
        }
    }
}
