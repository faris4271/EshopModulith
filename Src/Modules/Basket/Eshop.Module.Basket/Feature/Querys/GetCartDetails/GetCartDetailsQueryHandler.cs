using Eshop.Module.Basket.Dtos;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eshop.Module.Basket.Feature.Querys.GetCartDetails
{
    internal class GetCartDetailsQueryHandler : IQueryHandler<GetCartDetailsQuery, CartItemDto>
    {
        public Task<Result<CartItemDto>> Handle(GetCartDetailsQuery request, CancellationToken cancellationToken)
        {

             
        }
    }
}
 