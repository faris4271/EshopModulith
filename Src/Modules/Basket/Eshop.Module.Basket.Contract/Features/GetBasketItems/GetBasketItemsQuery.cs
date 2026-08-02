using Eshop.Module.Basket.Contract.Dtos;
using Shared.Contract.CQRS;

namespace Eshop.Module.Basket.Contract.Features.GetBasketItems
{
    public record GetBasketItemsQuery(Guid BasketId) : IQuery<List<CartItemDto>>;
}