using Eshop.Module.Basket.Contract.Dtos;
using Eshop.Module.Pricing.Contract.Dtos;

namespace Eshop.Module.Basket.Services
{
    public interface ICartService
    {
        Task<AddToCartResult> AddToCart(Guid customerId, Guid productId, int quantity);

        Task<UpdateCartResult> UpdateQuantity(CartQuantityUpdateDto cartQuantityUpdate);

        Task<CartDto> GetCartDetails(Guid customerId, CancellationToken cancellationToken);

        Task RemoveItem(Guid customerId, Guid productId);

        Task<CouponValidationResult> ApplyCoupon(Guid customerId, string couponCode);
        Task MigrateCart(Guid fromUserId, Guid toUserId);

        Task<decimal> CalculateBasketTotal(Guid customerId, CancellationToken cancellationToken = default);
    }
}
