using Module.Tax.Data;
using Module.Tax.Models;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Module.Tax.Feature.CreatTax;

public sealed class CreateTaxHandler(
    IGenericeRepository<TaxClass, TaxDbContext> repository) : ICommandHandler<CreateTaxCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateTaxCommand command, CancellationToken ct)
    {
        var taxClass = new TaxClass { Name = new Shared.DDD.Name(command.Name) };
        await repository.AddAsync(taxClass, ct);
        await repository.SaveChangesAsync(ct);
        return Result.Success(taxClass.Id);
    }
}

