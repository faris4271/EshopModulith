using CatalogContract.Dtos;
using Shared.Contract.CQRS;

namespace Catalog.Features.Brands.GetBrandById
{
    public record GetBrandByIdQuery(Guid id) : IQuery<GetBrandDto>;

}
