using Eshop.Module.Basket.Contract.Dtos;
using Shared.Contract.CQRS;


namespace Eshop.Module.Basket.Feature.Commands.CreatCartRule
{
    internal record CreatCartRuleCommand(CartRuleDto CartRuleDto) : ICommand;

}
