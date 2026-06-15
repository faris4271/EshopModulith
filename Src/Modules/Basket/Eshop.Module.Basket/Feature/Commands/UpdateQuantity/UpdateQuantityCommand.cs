using Eshop.Module.Basket.Contract.Dtos;
using Shared.Contract.CQRS;

namespace Eshop.Module.Basket.Feature.Commands.UpdateQuantity
{
    public record UpdateQuantityCommand(CartQuantityUpdateDto CartQuantityUpdateDto) : ICommand;

}
