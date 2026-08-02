using Module.Tax.Data;
using Module.Tax.Models;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Module.Tax.Feature.UpdateTax;

public sealed class UpdateTaxHandler(
    IGenericeRepository<TaxClass, TaxDbContext> repository) : ICommandHandler<UpdateTaxCommand>
{
    public async Task<Result> Handle(UpdateTaxCommand command, CancellationToken ct)
    {
        var taxClass = await repository.GetByIdAsync(command.Id, ct);
        if (taxClass is null) return Result.Failure(Error.NotFound("404", "Tax class not found"));

        taxClass.Name = new Shared.DDD.Name(command.Name);
        repository.Update(taxClass);
        await repository.SaveChangesAsync(ct);
        return Result.Success();
    }
}
