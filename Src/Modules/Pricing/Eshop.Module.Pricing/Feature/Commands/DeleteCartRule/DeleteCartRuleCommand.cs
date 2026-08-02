using Eshop.Module.Basket.Contract.Dtos;
using Shared.Contract.CQRS;

namespace Eshop.Module.Pricing.Feature.Commands.DeleteCartRule
{
    public record DeleteCartRuleCommand(Guid Id) : ICommand;
}
