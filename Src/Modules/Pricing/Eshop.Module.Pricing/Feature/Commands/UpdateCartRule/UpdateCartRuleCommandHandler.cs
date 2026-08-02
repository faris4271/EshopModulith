using Eshop.Module.Pricing.Models;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Eshop.Module.Pricing.Feature.Commands.UpdateCartRule
{
    internal class UpdateCartRuleCommandHandler(IGenericeRepository<CartRule, PricingDbContext> _cartRuleRepository) : ICommandHandler<UpdateCartRuleCommand>
    {
        public async Task<Result> Handle(UpdateCartRuleCommand model, CancellationToken cancellationToken)
        {
            var cartRule = await _cartRuleRepository.GetByIdAsync(model.Id, true, x => x.Products);
            if (cartRule == null)
            {
                return Result.Failure(Error.NotFound("CartRule", $"CartRule with Id {model.Id} not found"));
            }

            cartRule.Update(
                name: model.CartRuleDto.Name,
                description: model.CartRuleDto.Description,
                isActive: model.CartRuleDto.IsActive,
                startOn: model.CartRuleDto.StartOn,
                endOn: model.CartRuleDto.EndOn,
                isCouponRequired: model.CartRuleDto.IsCouponRequired,
                ruleToApply: model.CartRuleDto.RuleToApply,
                discountAmount: model.CartRuleDto.DiscountAmount,
                maxDiscountAmount: model.CartRuleDto.MaxDiscountAmount,
                discountStep: model.CartRuleDto.DiscountStep,
                usageLimitPerCoupon: model.CartRuleDto.UsageLimitPerCoupon,
                usageLimitPerCustomer: model.CartRuleDto.UsageLimitPerCustomer
            );

            cartRule.ClearProductRules();
            foreach (var product in model.CartRuleDto.Products)
            {
                cartRule.AddProductRule(product.Id);
            }

            await _cartRuleRepository.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
