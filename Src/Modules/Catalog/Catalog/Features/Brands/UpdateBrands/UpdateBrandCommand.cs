using CatalogContract.Dtos;
using Shared.Contract.CQRS;

namespace Catalog.Features.Brands.UpdateBrands
{
    public record UpdateBrandCommand(Guid id, CreatBrandDto CreatBrandDto) : ICommand;

}
