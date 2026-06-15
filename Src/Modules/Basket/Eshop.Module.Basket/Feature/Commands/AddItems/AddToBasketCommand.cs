using Eshop.Module.Basket.Contract.Dtos;
using Shared.Contract.CQRS;

namespace Eshop.Module.Basket.Feature.Commands.AddItems
{
    internal record AddToBasketCommand(Guid productId, int quantity) : ICommand<CartDto>;

}
