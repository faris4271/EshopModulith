using Microsoft.EntityFrameworkCore;
using Module.Tax.Data;
using Module.Tax.Feature.GetTax;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using Shared.Web.SmartTable;

namespace Module.Tax.Feature.GetTaxClassGrid
{
    internal class GetTaxClassGridHandler(IGenericeRepository<Models.TaxClass, TaxDbContext> _repository) : IQueryHandler<GetTaxClassGridQuery, SmartTableResult<TaxClassDto>>
    {
        public async Task<Result<SmartTableResult<TaxClassDto>>> Handle(GetTaxClassGridQuery request, CancellationToken cancellationToken)
        {
            var query = await _repository.Query();

            var param = request.smartTableParam;
            if (param.Search.PredicateObject != null)
            {
                dynamic search = param.Search.PredicateObject;
                if (search.Name != null)
                {
                    string searchName = search.Name;
                    query = query.Where(x => EF.Functions.ILike((string)(object)x.Name, $"%{searchName}%"));
                }


            }

            var result = query.ToSmartTableResult(param, x => new TaxClassDto
        (
            x.Id,
            x.Name.name
        ));
            return Result.Success(result);
        }
    }
}
