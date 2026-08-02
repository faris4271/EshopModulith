using CatalogContract.Dtos;
using Shared.Contract.CQRS;

namespace CatalogContract.Features.GetProductDiscuntable
{
    public record GetProductDiscuntableByCategoryIdsQuery(List<Guid> CategoryIds) : IQuery<List<DiscountableProductDto>>;

}
