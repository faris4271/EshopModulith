using CatalogContract.Dtos;
using Shared.Contract.CQRS;

namespace CatalogContract.Features.GetProductDiscuntable
{
    public record GetProductDiscuntableQuery(List<Guid> ProductIds) : IQuery<List<DiscountableProductDto>>;

}
