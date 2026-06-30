using CatalogContract.Dtos;
using Shared.Contract.CQRS;

namespace Catalog.Features.Brands.GetBrands
{
    public record GetBrandsQuery : IQuery<List<GetBrandDto>>;

}
