using CatalogContract.Dtos;
using Shared.Contract.CQRS;

namespace Catalog.Features.ProductOptions.CreateProductOption
{
    public record CreateProductOptionCommand(CreatProductOptionDto ProductOption) : ICommand;

}
