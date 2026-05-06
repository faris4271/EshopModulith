using Shared.DDD;

namespace Eshop.Module.Basket.Models
{
    public class CartRuleCustomerGroup:EntityBase<Guid>
    {
        public Guid CartRuleId { get; set; }

        public CartRule CartRule { get; set; }

        public Guid CustomerGroupId { get; set; }

      
    }
}