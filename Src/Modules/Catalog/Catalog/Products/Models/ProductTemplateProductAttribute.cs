using Shared.DDD;

namespace Catalog.Products.Models
{
    public class ProductTemplateProductAttribute : EntityBase<Guid>
    {
        public Guid ProductTemplateId { get; set; }

        public ProductTemplate ProductTemplate { get; set; }

        public Guid ProductAttributeId { get; set; }

        public ProductAttribute ProductAttribute { get; set; }
    }
}