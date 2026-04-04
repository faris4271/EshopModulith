using Shared.DDD;

namespace Catalog.Products.Models
{
    public class ProductAttributeGroup : EntityBase<Guid>
    {

        public Name Name { get; set; }

        public IList<ProductAttribute> Attributes { get; set; } = new List<ProductAttribute>();
    }
}