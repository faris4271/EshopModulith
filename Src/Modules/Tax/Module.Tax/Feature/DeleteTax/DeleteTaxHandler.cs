using Module.Tax.Data;
using Module.Tax.Models;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Module.Tax.Feature.DeleteTax;

public sealed class DeleteTaxHandler(
    IGenericeRepository<TaxClass, TaxDbContext> repository) : ICommandHandler<DeleteTaxCommand>
{
    public async Task<Result> Handle(DeleteTaxCommand command, CancellationToken ct)
    {
        var taxClass = await repository.GetByIdAsync(command.Id, ct);
        if (taxClass is null) return Result.Failure(Error.NotFound("404", "Tax class not found"));

        repository.Delete(taxClass);
        await repository.SaveChangesAsync(ct);
        return Result.Success();
    }
}
