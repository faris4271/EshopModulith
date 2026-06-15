using Eshop.Module.Basket.Contract.Dtos;
using Shared.Contract.CQRS;
using Shared.Web.SmartTable;


namespace Eshop.Module.Basket.Feature.Querys.GetCartRuleGrid
{
    public record GetCartRuleQuery(SmartTableParam SmartTableParam) : IQuery<SmartTableResult<CartRuleGridDto>>;

}
