using CatalogContract.Dtos;
using Shared.Contract.CQRS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Features.Products.Querys.GetProductDetails
{
    public record GetProductDetailsQuery(Guid Id):IQuery<ProductDetail>;
   
}
