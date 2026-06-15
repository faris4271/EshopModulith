using CatalogContract.Dtos;
using Eshop.Module.Basket.Contract.Dtos;
using Shared.Contract.CQRS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eshop.Module.Basket.Contract.Feature.GetProductsByIds
{
    public record GetProductsByIdsQuery(List<Guid> ProductIds) : IQuery<List<CartItemDto>>;
   

}
