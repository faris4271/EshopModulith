using Module.Tax.Data;
using Module.Tax.Models;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Module.Tax.Feature.CreateTaxRate;

public sealed class CreateTaxRateHandler(
    IGenericeRepository<TaxRate, TaxDbContext> repository) : ICommandHandler<CreateTaxRateCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateTaxRateCommand command, CancellationToken ct)
    {
        var taxRate = new TaxRate
        {
            TaxClassId = command.TaxClassId,
            CountryId = command.CountryId,
            StateOrProvinceId = command.StateOrProvinceId,
            Rate = command.Rate,
            ZipCode = command.ZipCode
        };
        await repository.AddAsync(taxRate, ct);
        await repository.SaveChangesAsync(ct);
        return Result.Success(taxRate.Id);
    }
}
