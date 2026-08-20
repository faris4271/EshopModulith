using Shared.Contract.CQRS;

namespace Eshop.Module.Pricing.Feature.Commands.DeleteCartRule
{
    public record DeleteCartRuleCommand(Guid Id) : ICommand;
}
