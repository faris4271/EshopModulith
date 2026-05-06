using Shared.Contract.CQRS;
using CatalogContract.Dtos;

namespace Catalog.Features.ProductAttributes.GetProductAttribute
{
    public record GetProductAttributeQuery(Guid Id) : IQuery<ProductAttributeDto>;
}
