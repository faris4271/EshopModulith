using Shared.Contract.CQRS;

namespace Catalog.Features.ProductAttributes.DeleteProductAttribute
{
    public record DeleteProductAttributeCommand(Guid Id) : ICommand;
}
