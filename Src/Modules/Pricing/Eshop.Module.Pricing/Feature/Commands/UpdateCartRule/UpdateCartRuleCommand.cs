using Eshop.Module.Basket.Contract.Dtos;
using Shared.Contract.CQRS;

namespace Eshop.Module.Pricing.Feature.Commands.UpdateCartRule
{
    public record UpdateCartRuleCommand(Guid Id, Contract.Dtos.CartRuleDto CartRuleDto) : ICommand;
}
