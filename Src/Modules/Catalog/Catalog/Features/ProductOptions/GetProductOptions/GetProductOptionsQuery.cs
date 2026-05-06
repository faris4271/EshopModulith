using CatalogContract.Dtos;
using Shared.Contract.CQRS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Features.ProductOptions.GetProductOptions
{
    public record GetProductOptionsQuery : IQuery<List<ProductOptionDto>>;
    
}
