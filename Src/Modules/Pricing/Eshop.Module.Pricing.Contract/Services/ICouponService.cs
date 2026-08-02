using Eshop.Module.Pricing.Contract.Dtos;
using System;
using System.Threading.Tasks;

namespace Eshop.Module.Pricing.Contract.Services
{
    public interface ICouponService
    {
        Task<CouponValidationResult> Validate(Guid customerId, string couponCode, CartInfoForCoupon cart);

        void AddCouponUsage(Guid customerId, Guid orderId, CouponValidationResult couponValidationResult);
    }
}
