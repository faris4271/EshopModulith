using Catalog.Features.Products.Querys.GetCartRuleProduct;
using Eshop.Module.Basket.Data;
using Eshop.Module.Basket.Dtos;
using Eshop.Module.Basket.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eshop.Module.Basket.Feature.Querys.GetCartRuleById
{
    internal class GetCartRuleByIdQueryHandler(
        IGenericeRepository<CartRule, BasketDbContext> _repository,
        ISender sender) : IQueryHandler<GetCartRuleByIdQuery, CartRuleDto>
    {
        public async Task<Result<CartRuleDto>> Handle(GetCartRuleByIdQuery request, CancellationToken cancellationToken)
        {

            var query = await _repository.GetAllAsQuerable();

            var cartRule = await query.Include(x => x.Products)
                .Include(x => x.Coupons).FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            var products = await sender.Send(new GetCartRuleProductQuery(cartRule.Products.Select(x => x.Id).ToList()));

            var cartRuleDto = new CartRuleDto
            {
                Id = cartRule.Id,
                Name = cartRule.Name,
                Description = cartRule.Description,
                IsActive = cartRule.IsActive,
                StartOn = cartRule.StartOn,
                EndOn = cartRule.EndOn,
                IsCouponRequired = cartRule.IsCouponRequired,
                RuleToApply = cartRule.RuleToApply.ToString(),
                DiscountAmount = cartRule.DiscountAmount,
                MaxDiscountAmount = cartRule.MaxDiscountAmount,
                DiscountStep = cartRule.DiscountStep,
                UsageLimitPerCoupon = cartRule.UsageLimitPerCoupon,
                UsageLimitPerCustomer = cartRule.UsageLimitPerCustomer,
                Products = products.Value.Select(x => new CartRuleProductDto
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList()

            };
           
            if (cartRule.IsCouponRequired)
            {
                var coupon = cartRule.Coupons.FirstOrDefault();
                if (coupon != null)
                {
                   cartRuleDto.CouponCode = coupon.Code;
                }
            }

            return Result.Success(cartRuleDto);

        }    
    }
}

