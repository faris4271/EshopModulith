using FluentValidation;

namespace Eshop.Module.Pricing.Feature.Commands.UpdateCartRule
{
    public class UpdateCartRuleValidator : AbstractValidator<UpdateCartRuleCommand>
    {
        public UpdateCartRuleValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.CartRuleDto.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.CartRuleDto.RuleToApply).NotEmpty();
            RuleFor(x => x.CartRuleDto.DiscountAmount).GreaterThanOrEqualTo(0);
            RuleFor(x => x.CartRuleDto.EndOn).GreaterThan(x => x.CartRuleDto.StartOn)
                .When(x => x.CartRuleDto.StartOn.HasValue && x.CartRuleDto.EndOn.HasValue);
        }
    }
}
