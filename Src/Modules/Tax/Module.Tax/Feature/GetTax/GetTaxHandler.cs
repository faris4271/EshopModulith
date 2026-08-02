using Module.Tax.Data;
using Module.Tax.Models;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Module.Tax.Feature.GetTax;

public sealed class GetTaxHandler(
    IGenericeRepository<TaxClass, TaxDbContext> repository) : IQueryHandler<GetTaxQuery, TaxClassDto>
{
    public async Task<Result<TaxClassDto>> Handle(GetTaxQuery query, CancellationToken ct)
    {
        var taxClass = await repository.GetByIdAsync(query.Id, ct);
        if (taxClass is null) return Result.Failure<TaxClassDto>(Error.NotFound("404", "Tax class not found"));
        return Result.Success(new TaxClassDto(taxClass.Id, taxClass.Name.name));
    }
}
