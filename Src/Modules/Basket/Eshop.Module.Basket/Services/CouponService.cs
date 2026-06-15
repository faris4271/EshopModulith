using Eshop.Module.Basket.Contract.Dtos;
using Eshop.Module.Basket.Data;
using Eshop.Module.Basket.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Shared.Abstraction;

namespace Eshop.Module.Basket.Services
{
    internal class CouponService(
        IGenericeRepository<Coupon, BasketDbContext> _couponRepository,
        IGenericeRepository<CartRuleUsage, BasketDbContext> _cartRuleUsageRepository
        ) : ICouponService
    {


        public async Task<CouponValidationResult> Validate(Guid customerId, string couponCode, CartInfoForCoupon cart)
        {
            var query = await _couponRepository.GetAllAsQuerable();

            var coupon = query.Include(x => x.CartRule).FirstOrDefault(x => x.Code == couponCode);
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

            var usageCount = await couponQuery.Where(x => x.CouponId == coupon.Id).CountAsync();

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

            return validationResult;

        }

        public void AddCouponUsage(Guid customerId, Guid orderId, CouponValidationResult couponValidationResult)
        {
            throw new NotImplementedException();
        }
    }
}
