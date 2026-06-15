using Eshop.Module.Basket.Contract.Dtos;

namespace Eshop.Module.Basket.Services
{
    public interface ICouponService
    {
        Task<CouponValidationResult> Validate(Guid customerId, string couponCode, CartInfoForCoupon cart);

        void AddCouponUsage(Guid customerId, Guid orderId, CouponValidationResult couponValidationResult);
    }
}
