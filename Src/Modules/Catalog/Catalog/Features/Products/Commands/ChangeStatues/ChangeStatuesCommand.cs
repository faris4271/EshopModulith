using Shared.Contract.CQRS;

namespace Catalog.Features.Products.Commands.ChangeStatues
{
    public record ChangeStatuesCommand(Guid id) : ICommand;

}
