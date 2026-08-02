using Eshop.Module.Basket.Contract.Dtos;
using Shared.Contract.CQRS;


namespace Eshop.Module.Pricing.Feature.Commands.CreatCartRule
{
    public record CreatCartRuleCommand(Contract.Dtos.CartRuleDto CartRuleDto) : ICommand;

}
