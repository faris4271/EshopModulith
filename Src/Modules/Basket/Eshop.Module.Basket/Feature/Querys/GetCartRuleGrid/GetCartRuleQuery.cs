using Eshop.Module.Basket.Dtos;
using Eshop.Module.Basket.Models;
using Shared.Contract.CQRS;
using Shared.Web.SmartTable;


namespace Eshop.Module.Basket.Feature.Querys.GetCartRuleGrid
{
    public record GetCartRuleQuery(SmartTableParam SmartTableParam) : IQuery<SmartTableResult<CartRuleGridDto>>;
   
}
