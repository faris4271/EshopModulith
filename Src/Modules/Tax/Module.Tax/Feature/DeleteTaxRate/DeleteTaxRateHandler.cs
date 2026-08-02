using Module.Tax.Data;
using Module.Tax.Models;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Module.Tax.Feature.DeleteTaxRate;

public sealed class DeleteTaxRateHandler(
    IGenericeRepository<TaxRate, TaxDbContext> repository) : ICommandHandler<DeleteTaxRateCommand>
{
    public async Task<Result> Handle(DeleteTaxRateCommand command, CancellationToken ct)
    {
        var taxRate = await repository.GetByIdAsync(command.Id, ct);
        if (taxRate is null) return Result.Failure(Error.NotFound("404", "Tax rate not found"));

        repository.Delete(taxRate);
        await repository.SaveChangesAsync(ct);
        return Result.Success();
    }
}
