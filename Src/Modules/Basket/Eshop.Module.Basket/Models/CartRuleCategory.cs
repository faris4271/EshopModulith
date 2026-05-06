using Shared.DDD;

namespace Eshop.Module.Basket.Models
{
    public class CartRuleCategory:EntityBase<Guid>
    {
        public Guid CategoryId { get; set; }


        public Guid CartRuleId { get; set; }

        public CartRule CartRule { get; set; }
    }
}