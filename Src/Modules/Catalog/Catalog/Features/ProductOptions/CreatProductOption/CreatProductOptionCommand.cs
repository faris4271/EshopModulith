using CatalogContract.Dtos;
using Shared.Contract.CQRS;

namespace Catalog.Features.ProductOptions.CreatProductOption
{
    public record CreateProductOptionCommand(CreatProductOptionDto ProductOption) : ICommand;

}
