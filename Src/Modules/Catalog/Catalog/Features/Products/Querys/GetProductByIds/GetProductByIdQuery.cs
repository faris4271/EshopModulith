using CatalogContract.Dtos;
using Shared.Contract.CQRS;

namespace Catalog.Features.Products.Querys.GetProductByIds
{
    public record GetProductByIdQuery(Guid id) : IQuery<ProductDto>;

}
