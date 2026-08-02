using Module.Tax.Data;
using Module.Tax.Models;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Module.Tax.Feature.UpdateTaxRate;

public sealed class UpdateTaxRateHandler(
    IGenericeRepository<TaxRate, TaxDbContext> repository) : ICommandHandler<UpdateTaxRateCommand>
{
    public async Task<Result> Handle(UpdateTaxRateCommand command, CancellationToken ct)
    {
        var taxRate = await repository.GetByIdAsync(command.Id, ct);
        if (taxRate is null) return Result.Failure(Error.NotFound("404", "Tax rate not found"));

        taxRate.Rate = command.Rate;
        repository.Update(taxRate);
        await repository.SaveChangesAsync(ct);
        return Result.Success();
    }
}
