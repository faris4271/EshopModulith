using Eshop.Module.Basket.Contract.Dtos;
using Eshop.Module.Basket.Contract.Services;
using Module.Identity.Contract.Services;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Eshop.Module.Basket.Feature.Commands.AddItems
{
    internal class AddToBasketCommandHandler(
       ICartService _cartService,
       ICurrentUserService _currentUserService
        )
        : ICommandHandler<AddToBasketCommand, CartDto>
    {
        public async Task<Result<CartDto>> Handle(AddToBasketCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.GetUserId();

            var result = await _cartService.AddToCart(currentUserId, request.productId, request.quantity);

            if (!result.Success)
                return Result.Failure<CartDto>(Error.Failure(result.ErrorCode, result.ErrorMessage.ToString()));


            var cartDetails = await _cartService.GetCartDetails(currentUserId);

            return Result.Success(cartDetails);
        }
    }
}