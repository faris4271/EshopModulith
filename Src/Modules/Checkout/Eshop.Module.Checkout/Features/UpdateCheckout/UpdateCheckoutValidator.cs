public sealed class UpdateCheckoutValidator : AbstractValidator<UpdateCheckoutCommand>
{
    public UpdateCheckoutValidator()
    {
        RuleFor(x => x.CheckoutId).NotEmpty();
        RuleFor(x => x.Status).NotEmpty().MaximumLength(50);
    }
}