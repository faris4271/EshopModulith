using Shared.DDD;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Eshop.Module.Basket.Models
{
    public class CartRule: EntityBase<Guid>
    {
        public CartRule(string name, string description, bool isActive, 
            DateTimeOffset? startOn, DateTimeOffset? endOn,
            bool isCouponRequired, string ruleToApply, decimal discountAmount,
            decimal? maxDiscountAmount, int? discountStep,
            int? usageLimitPerCoupon, int? usageLimitPerCustomer)
        {
            Name = name;
            Description = description;
            IsActive = isActive;
            StartOn = startOn;
            EndOn = endOn;
            IsCouponRequired = isCouponRequired;
            RuleToApply = ruleToApply;
            DiscountAmount = discountAmount;
            MaxDiscountAmount = maxDiscountAmount;
            DiscountStep = discountStep;
            UsageLimitPerCoupon = usageLimitPerCoupon;
            UsageLimitPerCustomer = usageLimitPerCustomer;
        }

        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(450)]
        public string Name { get;private set; }

        public string Description { get;private set; }

        public bool IsActive { get;private set; }

        public DateTimeOffset? StartOn { get;private set; }
        public DateTimeOffset? EndOn { get;private set; }

        public bool IsCouponRequired { get;private set; }

        [StringLength(450)]
        public string RuleToApply { get;private set; }

        public decimal DiscountAmount { get;private set; }

        public decimal? MaxDiscountAmount { get;private set; } = 0;
        public int? DiscountStep { get;private set; }

        public int? UsageLimitPerCoupon { get;private set; }

        public int? UsageLimitPerCustomer { get;private set; }
        public IList<Coupon> Coupons { get; set; } = new List<Coupon>();

        public IList<CartRuleCustomerGroup> CustomerGroups { get; set; } = new List<CartRuleCustomerGroup>();

        public IList<CartRuleProduct> Products { get; set; } = new List<CartRuleProduct>();

        public IList<CartRuleCategory> Categories { get; set; } = new List<CartRuleCategory>();

        public  void AddCoupon( string couponCode)
        {
            var coupon = new Coupon(couponCode, this);
            Coupons.Add(coupon);
        }     

        public void AddProductRule(Guid productId)
        {
            var productRule = new CartRuleProduct(this.Id, productId);

            Products.Add(productRule);
        }
    }
}
