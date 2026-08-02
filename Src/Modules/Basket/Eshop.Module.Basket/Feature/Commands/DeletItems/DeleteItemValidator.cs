using FluentValidation;
using Eshop.Module.Basket.Feature.Commands.DeletItems;

namespace Eshop.Module.Basket.Feature.Commands.DeletItems
{
    public class DeleteItemValidator : AbstractValidator<DeleteItemCommand>
    {
        public DeleteItemValidator()
        {
            RuleFor(x => x.CustomerId).NotEmpty();
            RuleFor(x => x.ProductId).NotEmpty();
        }
    }
}
