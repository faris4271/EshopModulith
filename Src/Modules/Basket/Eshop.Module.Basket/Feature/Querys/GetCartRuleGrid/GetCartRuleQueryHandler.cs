using Eshop.Module.Basket.Contract.Dtos;
using Eshop.Module.Basket.Data;
using Eshop.Module.Basket.Models;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using Shared.Web.SmartTable;

namespace Eshop.Module.Basket.Feature.Querys.GetCartRuleGrid
{
    internal class GetCartRuleQueryHandler(IGenericeRepository<CartRule, BasketDbContext> _repository)
        : IQueryHandler<GetCartRuleQuery, SmartTableResult<CartRuleGridDto>>
    {
        public async Task<Result<SmartTableResult<CartRuleGridDto>>> Handle(GetCartRuleQuery request, CancellationToken cancellationToken)
        {
            var query = await _repository.GetAllAsQuerable();


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
