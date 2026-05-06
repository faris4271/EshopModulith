using Shared.DDD;
using System.ComponentModel.DataAnnotations;

namespace Eshop.Module.Pricing.Models
{
    public class Coupon:EntityBase<Guid>
    {
        public Coupon()
        {
            CreatedOn = DateTimeOffset.Now;
        }

        public Guid CartRuleId { get; set; }

        public CartRule CartRule { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(450)]
        public string Code { get; set; }

        public DateTimeOffset CreatedOn { get; set; }
    }
}