using Module.Tax.Data;
using Module.Tax.Models;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Module.Tax.Feature.GetTaxRate;

public sealed class GetTaxRateHandler(
    IGenericeRepository<TaxRate, TaxDbContext> repository) : IQueryHandler<GetTaxRateQuery, TaxRateDto>
{
    public async Task<Result<TaxRateDto>> Handle(GetTaxRateQuery query, CancellationToken ct)
    {
        var taxRate = await repository.GetByIdAsync(query.Id, ct);
        if (taxRate is null) return Result.Failure<TaxRateDto>(Error.NotFound("404", "Tax rate not found"));
        return Result.Success(new TaxRateDto(taxRate.Id, taxRate.TaxClassId, taxRate.CountryId, taxRate.Rate));
    }
}
