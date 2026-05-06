using Shared.DDD;

namespace Eshop.Module.Basket.Models
{
    public class CartRuleProduct:EntityBase<Guid>
    {
        public CartRuleProduct(Guid cartRuleId, Guid productId)
        {
            CartRuleId = cartRuleId;
            ProductId = productId;
        }
        public Guid ProductId { get; set; }

        public Guid CartRuleId { get; set; }
        public CartRule CartRule { get; set; }
    }
}