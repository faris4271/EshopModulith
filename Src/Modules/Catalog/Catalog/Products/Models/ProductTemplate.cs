using Shared.DDD;

namespace Catalog.Products.Models
{
    public class ProductTemplate : EntityBase<Guid>
    {

        public Name Name { get; set; }

        public IList<ProductTemplateProductAttribute> ProductAttributes { get; protected set; } = new List<ProductTemplateProductAttribute>();

        public void AddAttribute(Guid attributeId)
        {
            var productTempateProductAttribute = new ProductTemplateProductAttribute
            {
                ProductTemplate = this,
                ProductAttributeId = attributeId
            };
            ProductAttributes.Add(productTempateProductAttribute);
        }

        public static ProductTemplate Create(string name)
        {
            var productTemplate = new ProductTemplate();
            productTemplate.Name = new Name(name);
            productTemplate.Id = Guid.NewGuid();
            return productTemplate;
        }


    }
}