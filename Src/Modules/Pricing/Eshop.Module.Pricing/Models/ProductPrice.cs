using Shared.DDD;

namespace Eshop.Module.Pricing.Models
{
    public class ProductPrice : EntityBase<Guid>
    {

        public Guid ProductId { get; private set; }
        public decimal BasePrice { get; private set; }
        public string BaseCurrency { get; private set; } = "USD";

        public DateTime EffectiveFromUtc { get; private set; }
        public DateTime? EffectiveToUtc { get; private set; }
        public bool IsActive { get; private set; }


        public ICollection<ProductTierPrice> TierPrices { get; private set; } = new List<ProductTierPrice>();
    }
}
