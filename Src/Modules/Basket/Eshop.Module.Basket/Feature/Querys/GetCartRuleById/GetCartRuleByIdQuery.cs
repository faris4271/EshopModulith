using Eshop.Module.Basket.Contract.Dtos;
using Shared.Contract.CQRS;

namespace Eshop.Module.Basket.Feature.Querys.GetCartRuleById
{
    internal record GetCartRuleByIdQuery(Guid Id) : IQuery<CartRuleDto>
    {
    }
}
