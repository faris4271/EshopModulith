using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Catalog.Features.Products.Commands.DeleteProduct
{
    public record DeleteProductCommand(Guid Id) : ICommand<Result>;
}
