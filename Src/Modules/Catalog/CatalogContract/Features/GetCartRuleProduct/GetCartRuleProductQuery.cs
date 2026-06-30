using CatalogContract.Dtos;
using Shared.Contract.CQRS;
using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogContract.Features.GetCartRuleProduct
{
    public record GetCartRuleProductQuery(List<Guid> productIds) : IQuery<List<GetCartRuleProductDto>>;
   
}
