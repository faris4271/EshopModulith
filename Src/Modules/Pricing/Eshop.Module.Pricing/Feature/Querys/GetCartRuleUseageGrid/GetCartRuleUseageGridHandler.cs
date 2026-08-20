using Eshop.Module.Pricing.Contract.Dtos;
using Eshop.Module.Pricing.Models;
using Microsoft.EntityFrameworkCore;
using Module.Identity.Contract.Services;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using Shared.Web.SmartTable;

namespace Eshop.Module.Pricing.Feature.Querys.GetCartRuleUseageGrid
{
    internal class GetCartRuleUseageGridHandler(IGenericeRepository<CartRuleUsage, PricingDbContext> _pricingRepository, IUserService _userService) : IQueryHandler<GetCartRuleUseageGridQuery, SmartTableResult<CartRuleUsageDto>>
    {
        public async Task<Result<SmartTableResult<CartRuleUsageDto>>> Handle(GetCartRuleUseageGridQuery request, CancellationToken cancellationToken)
        {
            var query = await _pricingRepository.Query();
            query = query.Include(x => x.CartRule).Include(x => x.Coupon);

            if (request.SmartTableParam.Search.PredicateObject != null)
            {
                dynamic search = request.SmartTableParam.Search.PredicateObject;
                if (search.RuleName != null)
                {
                    string ruleName = search.RuleName;
                    query = query.Where(x => x.CartRule.Name.Contains(ruleName));
                }

                if (search.CouponCode != null)
                {
                    string couponCode = search.CouponCode;
                    query = query.Where(x => x.Coupon.Code.Contains(couponCode));
                }


                if (search.CreatedOn != null)
                {
                    if (search.CreatedOn.before != null)
                    {
                        DateTimeOffset before = search.CreatedOn.before;
                        query = query.Where(x => x.CreatedOn <= before);
                    }

                    if (search.CreatedOn.after != null)
                    {
                        DateTimeOffset after = search.CreatedOn.after;
                        query = query.Where(x => x.CreatedOn >= after);
                    }
                }
            }

            var smartTableResult = query.ToSmartTableResult(request.SmartTableParam, x => new
            {
                x.Id,
                x.CartRuleId,
                CartRuleName = x.CartRule.Name,
                x.UserId,
                CouponCode = x.Coupon.Code,
                x.OrderId,
                x.CreatedOn
            });

            var userIds = smartTableResult.Items.Select(x => x.UserId.ToString()).Distinct().ToList();

            var usersDictionary = new Dictionary<string, string>();
            foreach (var id in userIds)
            {
                var user = await _userService.GetAsync(id, cancellationToken);
                if (user != null) usersDictionary[id] = user.FirstName;
            }

            var finalRecords = smartTableResult.Items.Select(x => new CartRuleUsageDto
            (
                x.Id,
                x.CartRuleId,
                x.CartRuleName,
                x.UserId,
                usersDictionary.TryGetValue(x.UserId.ToString(), out var firstName) ? firstName : "Unknown",
                x.CouponCode,
                x.OrderId,
                x.CreatedOn
            )).ToList();

            var result = new SmartTableResult<CartRuleUsageDto>
            {
                Items = finalRecords,
                TotalRecord = smartTableResult.TotalRecord
            };

            return Result.Success(result);
        }
    }
}
