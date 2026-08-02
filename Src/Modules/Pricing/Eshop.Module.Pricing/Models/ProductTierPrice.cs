using Shared.DDD;

namespace Eshop.Module.Pricing.Models
{

    public class ProductTierPrice : EntityBase<Guid>
    {

        public Guid ProductPriceId { get; private set; }
        public int MinQuantity { get; private set; }
        public decimal DiscountedPrice { get; private set; }
    }
}
