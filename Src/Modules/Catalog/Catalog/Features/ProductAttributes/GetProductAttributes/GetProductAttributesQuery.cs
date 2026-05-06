using Shared.Contract.CQRS;
using CatalogContract.Dtos;
using System.Collections.Generic;

namespace Catalog.Features.ProductAttributes.GetProductAttributes
{
    public record GetProductAttributesQuery : IQuery<IEnumerable<CreateProductAttributeDto>>;
}
