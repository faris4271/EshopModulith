using CatalogContract.Dtos;
using Shared.Contract.CQRS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Features.ProductOptions.GetProduct
{
    public record GetProductOptionQuery(Guid Id) : IQuery<ProductOptionDto>;

}
