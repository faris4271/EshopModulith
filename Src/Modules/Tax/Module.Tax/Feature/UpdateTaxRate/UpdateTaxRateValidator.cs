using FluentValidation;

namespace Module.Tax.Feature.UpdateTaxRate;

public sealed class UpdateTaxRateValidator : AbstractValidator<UpdateTaxRateCommand>
{
    public UpdateTaxRateValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Rate).GreaterThanOrEqualTo(0);
    }
}
