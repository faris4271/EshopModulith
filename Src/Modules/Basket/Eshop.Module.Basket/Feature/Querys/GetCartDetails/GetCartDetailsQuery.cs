using Eshop.Module.Basket.Dtos;
using Shared.Contract.CQRS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eshop.Module.Basket.Feature.Querys.GetCartDetails
{
    public record GetCartDetailsQuery() : IQuery<CartItemDto>;
    
}
