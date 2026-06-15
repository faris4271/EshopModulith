using Eshop.Module.Basket.Contract.Dtos;
using Eshop.Module.Basket.Contract.Services;
using EShop.Module.Core.Contract.Services;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Eshop.Module.Basket.Feature.Querys.GetCartList
{
    internal class GetCartListQueryHandler(ICartService _cartService, ICurrencyService currencyService) : IQueryHandler<GetCartListQuery, CartDto>
    {
        public async Task<Result<CartDto>> Handle(GetCartListQuery request, CancellationToken cancellationToken)
        {
            var cartItem = await _cartService.GetCartDetails(request.customerId);

            if (cartItem == null)
                cartItem = new CartDto(currencyService);

            return Result.Success(cartItem);


        }
    }
}
