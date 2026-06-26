using CatalogContract.Dtos;
using Shared.Contract.CQRS;

namespace Catalog.Features.ProductTemplet.GetProductTemplet
{
    public record GetTempletQuery(Guid id) : IQuery<ProductTempletDto>;

}
