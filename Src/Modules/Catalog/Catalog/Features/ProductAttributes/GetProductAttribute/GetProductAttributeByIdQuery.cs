using Shared.Contract.CQRS;
using CatalogContract.Dtos;

namespace Catalog.Features.ProductAttributes.GetProductAttribute
{
    public record GetProductAttributeByIdQuery(Guid Id) : IQuery<ProductAttributeDto>;
}
