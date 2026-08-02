using FluentValidation;

namespace Module.Tax.Feature.UpdateTax;

public sealed class UpdateTaxValidator : AbstractValidator<UpdateTaxCommand>
{
    public UpdateTaxValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}
