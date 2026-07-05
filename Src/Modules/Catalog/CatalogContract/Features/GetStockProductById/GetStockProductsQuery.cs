using CatalogContract.Dtos;
using Shared.Contract.CQRS;

namespace CatalogContract.Features.GetStockProducts
{
    public record GetStockProductsQuery(List<Guid> ProductId) : IQuery<List<GetProductStockDto>>;

}
