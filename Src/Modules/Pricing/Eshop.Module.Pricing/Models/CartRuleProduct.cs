using Shared.DDD;

namespace Eshop.Module.Pricing.Models
{
    public class CartRuleProduct : EntityBase<Guid>
    {
        public CartRuleProduct(Guid cartRuleId, Guid productId)
        {
            CartRuleId = cartRuleId;
            ProductId = productId;
        }

        public Guid ProductId { get; private set; }

        public Guid CartRuleId { get; private set; }

        public CartRule CartRule { get; set; }
    }
}