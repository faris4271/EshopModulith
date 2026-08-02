using Shared.DDD;

namespace Eshop.Module.Pricing.Models
{
    internal class CartRuleUsage : EntityBase<Guid>
    {
        public CartRuleUsage()
        {
            CreatedOn = DateTimeOffset.Now;
        }

        public Guid CartRuleId { get; set; }

        public CartRule CartRule { get; set; }

        public Guid? CouponId { get; set; }

        public Coupon Coupon { get; set; }

        public Guid UserId { get; set; }

        public Guid OrderId { get; set; }

        public DateTimeOffset CreatedOn { get; set; }
    }
}
