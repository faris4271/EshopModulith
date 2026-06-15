using CatalogContract.Dtos;
using Shared.Contract.CQRS;

namespace Catalog.Features.ProductOptions.UpdateProductOption
{
    public record UpdateProductOptionCommand(UpdateProductOptionDto ProductOption) : ICommand;

}
