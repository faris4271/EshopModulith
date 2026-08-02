using Eshop.Module.Pricing.Models;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Eshop.Module.Pricing.Feature.Commands.DeleteCartRule
{
    internal class DeleteCartRuleCommandHandler(IGenericeRepository<CartRule, PricingDbContext> _cartRuleRepository) : ICommandHandler<DeleteCartRuleCommand>
    {
        public async Task<Result> Handle(DeleteCartRuleCommand request, CancellationToken cancellationToken)
        {
            var cartRule = await _cartRuleRepository.GetByIdAsync(request.Id, true);
            if (cartRule == null)
            {
                return Result.Failure(Error.NotFound("CartRule", $"CartRule with Id {request.Id} not found"));
            }

            _cartRuleRepository.Delete(cartRule);
            await _cartRuleRepository.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
