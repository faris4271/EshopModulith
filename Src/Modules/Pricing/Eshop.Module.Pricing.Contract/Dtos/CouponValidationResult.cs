using Eshop.Module.Basket.Contract.Dtos;
using System;
using System.Collections.Generic;

namespace Eshop.Module.Pricing.Contract.Dtos
{
    public class CouponValidationResult
    {
        public bool Succeeded { get; set; }

        public decimal DiscountAmount { get; set; }

        public string CouponCode { get; set; }

        public string CouponRuleName { get; set; }

        public string ErrorMessage { get; set; }

        public Guid CouponId { get; set; }

        public CartRuleDto CartRule { get; set; }

        public IList<DiscountedProduct> DiscountedProducts { get; set; } = new List<DiscountedProduct>();
    }
}
