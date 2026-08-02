using Eshop.Module.Basket.Contract.Dtos;
using Shared.Contract.CQRS;
using Shared.Web.SmartTable;


namespace Eshop.Module.Pricing.Feature.Querys.GetCartRuleGrid
{
    public record GetCartRuleGridQuery(SmartTableParam SmartTableParam) : IQuery<SmartTableResult<CartRuleGridDto>>;

}
