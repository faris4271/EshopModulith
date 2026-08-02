using FluentValidation;

namespace Eshop.Module.Basket.Feature.Commands.UpdateQuantity
{
    public class UpdateQuantityValidator : AbstractValidator<UpdateQuantityCommand>
    {
        public UpdateQuantityValidator()
        {
            RuleFor(x => x.CartQuantityUpdateDto.cartItemId).NotEmpty();
            RuleFor(x => x.CartQuantityUpdateDto.Quantity).GreaterThan(0);
        }
    }
}
