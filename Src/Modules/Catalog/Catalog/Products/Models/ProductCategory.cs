using Shared.DDD;

namespace Catalog.Products.Models
{
    public class ProductCategory : EntityBase<Guid>
    {
        public bool IsFeaturedProduct { get; set; }

        public int DisplayOrder { get; set; }

        public Guid CategoryId { get; set; }

        public Guid ProductId { get; set; }

        public Category.Models.Category Category { get; set; }

        public Product Product { get; set; }
    }
}