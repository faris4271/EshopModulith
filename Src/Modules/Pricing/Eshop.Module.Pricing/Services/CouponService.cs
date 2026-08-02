using CatalogContract.Dtos;
using CatalogContract.Features.GetProductDiscuntable;
using Eshop.Module.Pricing.Contract.Dtos;
using Eshop.Module.Pricing.Contract.Services;
using Eshop.Module.Pricing.Models;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Abstraction;
using CartInfoForCoupon = Eshop.Module.Pricing.Contract.Dtos.CartInfoForCoupon;

namespace Eshop.Module.Pricing.Services
{
    internal class CouponService(
        IGenericeRepository<Coupon, PricingDbContext> _couponRepository,
        IGenericeRepository<CartRuleUsage, PricingDbContext> _cartRuleUsageRepository,
        ISender _sender
        ) : ICouponService
    {


        public async Task<CouponValidationResult> Validate(Guid customerId, string couponCode, CartInfoForCoupon cart)
        {
            var query = await _couponRepository.GetAllAsQuerable();

            var coupon = await query.Include(x => x.CartRule).FirstOrDefaultAsync(x => x.Code == couponCode);
            var validationResult = new CouponValidationResult { Succeeded = false };
            if (coupon == null || !coupon.CartRule.IsActive)
            {
                validationResult.ErrorMessage = $"The coupon {couponCode} is not exist.";
                return validationResult;
            }

            if (coupon.CartRule.StartOn.HasValue && coupon.CartRule.StartOn > DateTimeOffset.Now)
            {
                validationResult.ErrorMessage = $"The coupon {couponCode} should be used after {coupon.CartRule.StartOn}.";
                return validationResult;
            }

            if (coupon.CartRule.EndOn.HasValue && coupon.CartRule.EndOn <= DateTimeOffset.Now)
            {
                validationResult.ErrorMessage = $"The coupon {couponCode} is expired.";
                return validationResult;
            }

            var couponQuery = await _cartRuleUsageRepository.Query();

            var usageCount = await couponQuery.CountAsync(x => x.CouponId == coupon.Id);

            if (coupon.CartRule.UsageLimitPerCoupon.HasValue && usageCount >= coupon.CartRule.UsageLimitPerCoupon)
            {
                validationResult.ErrorMessage = $"The coupon {couponCode} has reached its usage limit.";
                return validationResult;
            }

            var couponUsageByCustomer = await couponQuery.Where(x => x.CouponId == coupon.Id && x.UserId == customerId).CountAsync();

            if (coupon.CartRule.UsageLimitPerCustomer.HasValue && couponUsageByCustomer >= coupon.CartRule.UsageLimitPerCustomer)
            {
                validationResult.ErrorMessage = $"You have reached the usage limit for the coupon {couponCode}.";
                return validationResult;
            }
            var discountedProducts = new List<DiscountableProductDto>();

            if (!coupon.CartRule.Products.Any() && !coupon.CartRule.Categories.Any())
            {
                var productId = cart.Items.Select(x => x.ProductId).ToList();

                var products = await _sender.Send(new GetProductDiscuntableQuery(productId));

                if (!products.IsSuccess)
                {
                    validationResult.ErrorMessage = $"Failed to retrieve product information for coupon validation.";
                    return validationResult;
                }

                discountedProducts = products.Value;

            }
            else
            {
                discountedProducts = await GetDiscountableProduct(coupon.CartRule.Products, coupon.CartRule.Categories);
            }

            var discountableDict = discountedProducts
               .GroupBy(x => x.Id)
                .ToDictionary(g => g.Key, g => g.First());

            foreach (var item in cart.Items)
            {

                if (discountableDict.TryGetValue(item.ProductId, out var discountableProduct))
                {
                    validationResult.DiscountedProducts.Add(new DiscountedProduct
                    {
                        Id = discountableProduct.Id,
                        Name = discountableProduct.Name,
                        Price = discountableProduct.Price,
                        Quantity = item.Quantity
                    });
                }
            }
            if (!validationResult.DiscountedProducts.Any())
            {
                validationResult.ErrorMessage = $"The coupon {couponCode} doesn't apply to any products in your cart";
                return validationResult;
            }

            validationResult.Succeeded = true;
            validationResult.CouponId = coupon.Id;
            validationResult.CouponCode = coupon.Code;
            validationResult.CouponRuleName = coupon.CartRule.Name;
            validationResult.CartRule = coupon.CartRule.Adapt<CartRuleDto>();

            switch (coupon.CartRule.RuleToApply)
            {
                case "cart_fixed":
                    validationResult.DiscountAmount = Math.Min(coupon.CartRule.DiscountAmount, coupon.CartRule.MaxDiscountAmount.GetValueOrDefault(decimal.MaxValue));
                    return validationResult;

                case "by_percent":
                    var maxDiscountAmount = coupon.CartRule.MaxDiscountAmount.GetValueOrDefault(decimal.MaxValue);
                    foreach (var item in validationResult.DiscountedProducts)
                    {
                        item.DiscountAmount = Math.Min((item.Price * coupon.CartRule.DiscountAmount / 100) * item.Quantity, maxDiscountAmount);
                        maxDiscountAmount -= item.DiscountAmount;
                    }

                    validationResult.DiscountAmount = validationResult.DiscountedProducts.Sum(x => x.DiscountAmount);
                    return validationResult;

                default:
                    throw new InvalidOperationException($"{coupon.CartRule.RuleToApply} is not supported");
            }

            return validationResult;

        }

        public void AddCouponUsage(Guid customerId, Guid orderId, CouponValidationResult couponValidationResult)
        {
            if (!couponValidationResult.Succeeded || couponValidationResult.CartRule == null)
            {
                return;
            }

            CartRuleUsage couponUsage;
            switch (couponValidationResult.CartRule.RuleToApply)
            {
                case "cart_fixed":
                    couponUsage = new CartRuleUsage
                    {
                        UserId = customerId,
                        OrderId = orderId,
                        CouponId = couponValidationResult.CouponId,
                        CartRuleId = couponValidationResult.CartRule.Id
                    };

                    _cartRuleUsageRepository.Add(couponUsage);
                    break;

                case "by_percent":
                    foreach (var item in couponValidationResult.DiscountedProducts)
                    {
                        for (var i = 0; i < item.Quantity; i++)
                        {
                            couponUsage = new CartRuleUsage
                            {
                                UserId = customerId,
                                OrderId = orderId,
                                CouponId = couponValidationResult.CouponId,
                                CartRuleId = couponValidationResult.CartRule.Id
                            };

                            _cartRuleUsageRepository.Add(couponUsage);
                        }
                    }

                    break;

                default:
                    throw new InvalidOperationException($"{couponValidationResult.CartRule.RuleToApply} is not supported");
            }
        }
        private async Task<List<DiscountableProductDto>> GetDiscountableProduct(IList<CartRuleProduct> cartRuleProducts, IList<CartRuleCategory> cartRuleCategorys)
        {
            var discountedProducts = new List<DiscountableProductDto>();

            if (cartRuleProducts.Any())
            {
                var productIds = cartRuleProducts.Select(x => x.ProductId).ToList();

                var products = await _sender.Send(new GetProductDiscuntableQuery(productIds));

                if (!products.IsSuccess)
                {
                    throw new Exception("Failed to retrieve product information for coupon validation.");
                }
                discountedProducts = products.Value;
            }

            if (cartRuleCategorys.Any())
            {
                var categoryIds = cartRuleCategorys.Select(x => x.CategoryId).ToList();

                var products = await _sender.Send(new GetProductDiscuntableByCategoryIdsQuery(categoryIds));
                if (!products.IsSuccess)
                    throw new Exception("Failed to retrieve product information for coupon validation.");

                discountedProducts = products.Value;

            }

            return discountedProducts;
        }
    }
}
