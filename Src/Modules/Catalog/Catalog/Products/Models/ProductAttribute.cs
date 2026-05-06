using Shared.DDD;

namespace Catalog.Products.Models
{
    public class ProductAttribute : EntityBase<Guid>
    {

        public Name Name { get; set; }

        public Guid GroupId { get; set; }

        public ProductAttributeGroup Group { get; set; }

        public IList<ProductTemplateProductAttribute> ProductTemplates { get; protected set; } = new List<ProductTemplateProductAttribute>();
    }
}