using Module.Tax.Data;
using Module.Tax.Feature.GetTax;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Module.Tax.Feature.GetTaxList
{
    internal class GetTaxClassListHandler(IGenericeRepository<Models.TaxClass, TaxDbContext> _repository) : IQueryHandler<GetTaxClassListQuery, List<TaxClassDto>>
    {
        public async Task<Result<List<TaxClassDto>>> Handle(GetTaxClassListQuery request, CancellationToken cancellationToken)
        {
            var query = await _repository.Query();

            var taxClasses = query.Select(x => new TaxClassDto(x.Id, x.Name.name)).ToList();

            return Result.Success(taxClasses);
        }
    }
}
