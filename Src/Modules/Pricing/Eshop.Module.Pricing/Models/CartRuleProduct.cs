using Shared.DDD;

namespace Eshop.Module.Pricing.Models
{
    public class CartRuleProduct:EntityBase<Guid>
    {
        public Guid ProductId { get; set; }

        public Guid CartRuleId { get; set; }
        public CartRule CartRule { get; set; }
    }
}