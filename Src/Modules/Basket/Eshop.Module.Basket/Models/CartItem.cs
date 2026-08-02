using Shared.DDD;

namespace Eshop.Module.Basket.Models
{
    public class CartItem : Aggregate<Guid>, IAuditableEntity
    {




        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        public string CustomerId { get; set; }

        public Guid? VendorId { get; set; }

        public string? CreatedById { get; set; }

        public string? LatestUpdatedById { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public DateTimeOffset? LatestUpdatedOn { get; set; }
    }
}
