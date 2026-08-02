using Eshop.Module.Pricing.Contract.Dtos;
using Shared.Contract.CQRS;

namespace Eshop.Module.Pricing.Feature.Querys.GetCartRuleById
{
    internal record GetCartRuleByIdQuery(Guid Id) : IQuery<CartRuleDto>
    {
    }
}
