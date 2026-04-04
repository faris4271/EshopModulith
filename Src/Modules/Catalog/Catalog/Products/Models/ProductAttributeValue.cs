using Shared.DDD;

namespace Catalog.Products.Models
{
    public class ProductAttributeValue : EntityBase<Guid>
    {

        public Guid AttributeId { get; set; }

        public ProductAttribute Attribute { get; set; }

        public Guid ProductId { get; set; }

        public Product Product { get; set; }

        public string Value { get; set; }
    }
}