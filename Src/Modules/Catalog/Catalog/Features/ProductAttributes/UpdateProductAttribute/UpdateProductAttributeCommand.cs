using Shared.Contract.CQRS;
using CatalogContract.Dtos;

namespace Catalog.Features.ProductAttributes.UpdateProductAttribute
{
    public record UpdateProductAttributeCommand(Guid Id, UpdateProductAttributeDto productAttributeDto) : ICommand;
}
