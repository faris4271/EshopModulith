using Eshop.Module.Basket.Models;

namespace Eshop.Module.Basket.Services
{
    public class CouponValidationResult
    {
        public bool Succeeded { get; set; }

        public decimal DiscountAmount { get; set; }

        public string CouponCode { get; set; }

        public string CouponRuleName { get; set; }

        public string ErrorMessage { get; set; }

        public long CouponId { get; set; }

        public CartRule CartRule { get; set; }

        public IList<DiscountedProduct> DiscountedProducts { get; set; } = new List<DiscountedProduct>();
    }
}