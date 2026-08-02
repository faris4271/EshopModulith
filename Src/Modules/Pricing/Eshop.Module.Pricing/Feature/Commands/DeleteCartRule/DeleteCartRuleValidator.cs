using FluentValidation;

namespace Eshop.Module.Pricing.Feature.Commands.DeleteCartRule
{
    public class DeleteCartRuleValidator : AbstractValidator<DeleteCartRuleCommand>
    {
        public DeleteCartRuleValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
