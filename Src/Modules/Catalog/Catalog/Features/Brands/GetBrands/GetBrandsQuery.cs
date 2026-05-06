using Catalog.Brands.Dtos;
using CatalogContract.Dtos;
using Shared.Contract.CQRS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Features.Brands.GetBrands
{
    public record GetBrandsQuery : IQuery<List<GetBrandDto>>;
  
}
