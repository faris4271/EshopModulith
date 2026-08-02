using FluentValidation;

namespace Eshop.Module.Basket.Feature.Commands.AddItems
{
    public class AddToBasketValidator : AbstractValidator<AddToBasketCommand>
    {
        public AddToBasketValidator()
        {
            RuleFor(x => x.productId).NotEmpty();
            RuleFor(x => x.quantity).GreaterThan(0);
        }
    }
}
