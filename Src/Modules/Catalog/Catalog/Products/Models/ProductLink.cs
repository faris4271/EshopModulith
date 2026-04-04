using Shared.DDD;

namespace Catalog.Products.Models
{
    public class ProductLink : EntityBase<Guid>
    {
        public Guid ProductId { get; set; }

        public Product Product { get; set; }

        public Guid LinkedProductId { get; set; }

        public Product LinkedProduct { get; set; }

        public ProductLinkType LinkType { get; set; }
    }
}