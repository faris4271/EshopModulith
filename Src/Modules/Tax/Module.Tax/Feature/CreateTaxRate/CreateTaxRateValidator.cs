using FluentValidation;

namespace Module.Tax.Feature.CreateTaxRate;

public sealed class CreateTaxRateValidator : AbstractValidator<CreateTaxRateCommand>
{
    public CreateTaxRateValidator()
    {
        RuleFor(x => x.TaxClassId).NotEmpty();
        RuleFor(x => x.CountryId).NotEmpty();
        RuleFor(x => x.Rate).GreaterThanOrEqualTo(0);
    }
}
