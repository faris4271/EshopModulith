using System;
using System.Collections.Generic;
using System.Text;

namespace Eshop.Module.Basket.Contract.Dtos
{
    public class CouponValidationResult
    {
        public bool Succeeded { get; set; }

        public decimal DiscountAmount { get; set; }

        public string CouponCode { get; set; }

        public string CouponRuleName { get; set; }

        public string ErrorMessage { get; set; }

        public long CouponId { get; set; }

        public CartRuleDto CartRule { get; set; }

        public IList<DiscountedProductDto> DiscountedProducts { get; set; } = new List<DiscountedProductDto>();
    }
}
