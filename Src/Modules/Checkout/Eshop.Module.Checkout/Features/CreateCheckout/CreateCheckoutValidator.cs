

public sealed class CreateCheckoutValidator : AbstractValidator<CreateCheckoutCommand>
{
    public CreateCheckoutValidator()
    {
        RuleFor(x => x.BasketId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.TotalAmount).GreaterThan(0);
    }
}