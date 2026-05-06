

using Shared.Contract.CQRS;

namespace Catalog.Features.ProductOptions.DeletProductOption
{
    public record DeletProductOptionCommand(Guid id) : ICommand;

}
