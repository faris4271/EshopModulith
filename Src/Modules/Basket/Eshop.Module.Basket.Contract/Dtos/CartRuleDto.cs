using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Eshop.Module.Basket.Contract.Dtos
{
    public class CartRuleDto
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public DateTimeOffset? StartOn { get; set; }

        public DateTimeOffset? EndOn { get; set; }

        public bool IsCouponRequired { get; set; }

        public string CouponCode { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        public string RuleToApply { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        public decimal DiscountAmount { get; set; }

        public decimal? MaxDiscountAmount { get; set; }

        public int? DiscountStep { get; set; }

        public int? UsageLimitPerCoupon { get; set; }

        public int? UsageLimitPerCustomer { get; set; }

        public IList<CartRuleProductDto> Products { get; set; } = new List<CartRuleProductDto>();
    }
}
