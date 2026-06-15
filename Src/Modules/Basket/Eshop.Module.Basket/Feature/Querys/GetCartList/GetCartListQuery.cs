
using Eshop.Module.Basket.Contract.Dtos;
using Shared.Contract.CQRS;

namespace Eshop.Module.Basket.Feature.Querys.GetCartList
{
    internal record GetCartListQuery(Guid customerId) : IQuery<CartDto>;

}
