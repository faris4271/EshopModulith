using Eshop.Module.Basket.Contract.Dtos;
using Eshop.Module.Pricing.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using Shared.Web.SmartTable;

namespace Eshop.Module.Pricing.Feature.Querys.GetCartRuleGrid
{
    internal class GetCartRuleGridHandler(IGenericeRepository<CartRule, PricingDbContext> _repository)
        : IQueryHandler<GetCartRuleGridQuery, SmartTableResult<CartRuleGridDto>>
    {
        public async Task<Result<SmartTableResult<CartRuleGridDto>>> Handle(GetCartRuleGridQuery request, CancellationToken cancellationToken)
        {
            var query = await _repository.GetAllAsQuerable();

            if (request.SmartTableParam.Search.PredicateObject != null)
            {
                dynamic search = request.SmartTableParam.Search.PredicateObject;
                if (search.Name != null)
                {
                    string searchName = search.Name;
                    query = query.Where(x => EF.Functions.ILike((string)(object)x.Name, $"%{searchName}%"));
                }
            }

            var cartRules = query.ToSmartTableResult(
            request.SmartTableParam,
            cartRule => new CartRuleGridDto
            {
                Id = cartRule.Id,
                Name = cartRule.Name,
                StartOn = cartRule.StartOn,
                EndOn = cartRule.EndOn,
                IsActive = cartRule.IsActive
            });

            return Result.Success(cartRules);
        }
    }
}
