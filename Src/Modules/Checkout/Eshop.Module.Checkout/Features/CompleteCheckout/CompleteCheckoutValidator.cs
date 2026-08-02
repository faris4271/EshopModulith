using FluentValidation;

public sealed class CompleteCheckoutValidator : AbstractValidator<CompleteCheckoutCommand>
{
    public CompleteCheckoutValidator()
    {
        RuleFor(x => x.CheckoutId).NotEmpty();
    }
}