using Eshop.Module.Basket.Contract.Dtos;

namespace Eshop.Module.Basket.Contract.Services
{
    public interface ICartService
    {
        Task<AddToCartResult> AddToCart(Guid customerId, Guid productId, int quantity);

        Task<UpdateCartResult> UpdateQuantity(CartQuantityUpdateDto cartQuantityUpdate);

        Task<CartDto> GetCartDetails(Guid customerId);

        Task RemoveItem(Guid customerId, Guid productId);

        Task<CouponValidationResult> ApplyCoupon(Guid customerId, string couponCode);
        Task MigrateCart(Guid fromUserId, Guid toUserId);
    }
}
