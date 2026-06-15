using Shared.Contract.CQRS;

namespace Eshop.Module.Basket.Feature.Commands.DeletItems
{
    public record DeleteItemCommand(Guid CustomerId, Guid ProductId) : ICommand;

}
