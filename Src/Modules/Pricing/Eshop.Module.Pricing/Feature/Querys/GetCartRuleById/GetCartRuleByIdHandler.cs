using CatalogContract.Features.GetCartRuleProduct;
using Eshop.Module.Pricing.Contract.Dtos;
using Eshop.Module.Pricing.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Eshop.Module.Pricing.Feature.Querys.GetCartRuleById
{
    internal class GetCartRuleByIdHandler(
        IGenericeRepository<CartRule, PricingDbContext> _repository,
        ISender sender) : IQueryHandler<GetCartRuleByIdQuery, CartRuleDto>
    {
        public async Task<Result<CartRuleDto>> Handle(GetCartRuleByIdQuery request, CancellationToken cancellationToken)
        {

            var query = await _repository.GetAllAsQuerable();

            var cartRule = await query.Include(x => x.Products)
                .Include(x => x.Coupons).FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            var products = await sender.Send(new GetCartRuleProductQuery(cartRule.Products.Select(x => x.ProductId).ToList()));

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

