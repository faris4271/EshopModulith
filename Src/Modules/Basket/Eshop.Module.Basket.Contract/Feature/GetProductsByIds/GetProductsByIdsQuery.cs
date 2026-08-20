using CatalogContract.Dtos;
using Shared.Contract.CQRS;

namespace Eshop.Module.Basket.Contract.Feature.GetProductsByIds
{
    public record GetProductsByIdsQuery(List<Guid> ProductIds) : IQuery<List<CartItemDto>>;


}
