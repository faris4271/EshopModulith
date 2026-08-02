using FluentValidation;

namespace Module.Tax.Feature.CreatTax;

public sealed class CreateTaxValidator : AbstractValidator<CreateTaxCommand>
{
    public CreateTaxValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}
