using Shared.DDD;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Products.Models
{
    public class ProductAttributeValue : EntityBase<Guid>
    {
        public ProductAttributeValue(Guid attributeId, string value)
        {
            Id = Guid.NewGuid();
            AttributeId = attributeId;
            Value = value;
        }

        public Guid AttributeId { get; private set; }

        [Required]
        [StringLength(500)]
        public string Value { get; private set; }

        public Guid? ProductId { get; set; }

        public ProductAttribute Attribute { get; set; }

        public Product Product { get; set; }
    }
}