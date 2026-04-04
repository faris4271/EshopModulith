using Shared.DDD;

namespace Catalog.Products.Models
{
    public class ProductMedia : EntityBase<Guid>
    {
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public Guid MediaId { get; set; }
        public int DisplayOrder { get; set; }

    }
}
