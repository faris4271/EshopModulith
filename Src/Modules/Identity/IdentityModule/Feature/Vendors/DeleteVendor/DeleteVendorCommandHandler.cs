using IdentityModule.Data;
using IdentityModule.Domain;
using Module.Identity.Contract.Feature.Vendors.DeleteVendor;
using EShop.Module.Core.Contract.Services;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace IdentityModule.Feature.Vendors.DeleteVendor;

public class DeleteVendorCommandHandler(
    IGenericeRepository<Vendor, IdentityDbContext> vendorRepository,
    IEntityService entityService) : ICommandHandler<DeleteVendorCommand, Guid>
{
    const string EntityType = "Vendor";

    public async Task<Result<Guid>> Handle(DeleteVendorCommand request, CancellationToken cancellationToken)
    {
        var vendor = await vendorRepository.GetByIdAsync(request.Id);

        if (vendor is null)
        {
            return Result.Failure<Guid>(Error.NullValue);
        }

        using var transaction = vendorRepository.BeginTransaction();

        try
        {
            vendorRepository.Delete(vendor);

            await entityService.Remove(request.Id, EntityType);

            await transaction.CommitAsync(cancellationToken);

            await vendorRepository.SaveChangesAsync(cancellationToken);

            return Result.Success(request.Id);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return Result.Failure<Guid>(Error.Problem("400", ex.Message));
        }
    }
}