using Eshop.Module.Pricing.Contract.Dtos;
using Shared.Contract.CQRS;
using Shared.Web.SmartTable;

namespace Eshop.Module.Pricing.Feature.Querys.GetCartRuleUseageGrid
{
    public record GetCartRuleUseageGridQuery(SmartTableParam SmartTableParam) : IQuery<SmartTableResult<CartRuleUsageDto>>
    {
    }
}
