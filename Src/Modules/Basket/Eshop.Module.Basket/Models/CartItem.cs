using Shared.DDD;

namespace Eshop.Module.Basket.Models
{
    public class CartItem : Aggregate<Guid>
    {
        public CartItem() 
        {
            CreatedOn = DateTimeOffset.Now;
            LatestUpdatedOn = DateTimeOffset.Now;
        }

        public DateTimeOffset CreatedOn { get; set; }

        public DateTimeOffset LatestUpdatedOn { get; set; }

        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        public string CustomerId { get; set; }

        public long? VendorId { get; set; }

    }
}
