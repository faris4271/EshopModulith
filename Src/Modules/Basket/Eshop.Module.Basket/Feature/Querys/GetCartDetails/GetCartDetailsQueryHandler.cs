using Eshop.Module.Basket.Contract.Dtos;
using Eshop.Module.Basket.Contract.Services;
using Module.Identity.Contract.Services;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Eshop.Module.Basket.Feature.Querys.GetCartDetails
{
    internal class GetCartDetailsQueryHandler(
           ICartService _cartService,
            ICurrentUserService _currentUserService
        ) : IQueryHandler<GetCartDetailsQuery, CartDto>
    {
        public async Task<Result<CartDto>> Handle(GetCartDetailsQuery request, CancellationToken cancellationToken)
        {


            var cartDetails = await _cartService.GetCartDetails(request.CustomerId);

            if (cartDetails == null)
                return Result.Failure<CartDto>(new Error("CartNotFound", "Cart not found", ErrorType.NotFound));

            return Result.Success(cartDetails);


        }
    }
}
