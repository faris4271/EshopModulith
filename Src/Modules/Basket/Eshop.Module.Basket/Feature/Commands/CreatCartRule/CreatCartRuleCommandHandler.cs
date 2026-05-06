using Eshop.Module.Basket.Data;
using Eshop.Module.Basket.Models;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eshop.Module.Basket.Feature.Commands.CreatCartRule
{
    internal class CreatCartRuleCommandHandler(IGenericeRepository<CartRule,BasketDbContext> _cartRuleRepository) : ICommandHandler<CreatCartRuleCommand>
    {
        public async Task<Result> Handle(CreatCartRuleCommand model, CancellationToken cancellationToken)
        {
            var cartRule = new CartRule
            (
                name : model.CartRuleDto.Name,
                description : model.CartRuleDto.Description,
                isActive : model.CartRuleDto.IsActive,
                startOn : model.CartRuleDto.StartOn,
                endOn : model.CartRuleDto.EndOn,
                isCouponRequired : model.CartRuleDto.IsCouponRequired,
                ruleToApply : model.CartRuleDto.RuleToApply,
                discountAmount : model.CartRuleDto.DiscountAmount,
                discountStep : model.CartRuleDto.DiscountStep,
                maxDiscountAmount : model.CartRuleDto.MaxDiscountAmount,
                usageLimitPerCoupon : model.CartRuleDto.UsageLimitPerCoupon,
                usageLimitPerCustomer : model.CartRuleDto.UsageLimitPerCustomer
            );

            if(cartRule.IsCouponRequired && !string.IsNullOrEmpty(model.CartRuleDto.CouponCode))
            {
                cartRule.AddCoupon(model.CartRuleDto.CouponCode);
            }
            foreach(var product in model.CartRuleDto.Products)
            {
                cartRule.AddProductRule(product.Id);
            }

            await _cartRuleRepository.AddAsync(cartRule);

            await _cartRuleRepository.SaveChangesAsync();

            return Result.Success();

        }

    }
}
