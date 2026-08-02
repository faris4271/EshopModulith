using IdentityModule.Data;
using IdentityModule.Domain;
using Module.Identity.Contract.Dtos;
using Module.Identity.Contract.Feature.Vendors.UpdateVendor;
using EShop.Module.Core.Contract.Services;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace IdentityModule.Feature.Vendors.UpdateVendor;

public class UpdateVendorCommandHandler(
    IGenericeRepository<Vendor, IdentityDbContext> vendorRepository,
    IEntityService entityService) : ICommandHandler<UpdateVendorCommand>
{
    const string EntityType = "Vendor";

    public async Task<Result> Handle(UpdateVendorCommand request, CancellationToken cancellationToken)
    {
        var vendor = await vendorRepository.GetByIdAsync(request.Id);

        if (vendor is null)
        {
            return Result.Failure(Error.NullValue);
        }

        var safeSlug = await entityService.ToSafeSlug(request.VendorDto.Slug, vendor.Id, EntityType);

        await entityService.Update(request.VendorDto.Name, safeSlug, vendor.Id, EntityType);

        vendor.Update(
            request.VendorDto.Name,
            request.VendorDto.Description,
            request.VendorDto.Email,
            request.VendorDto.IsActive,
            safeSlug);

        vendorRepository.Update(vendor);

        await vendorRepository.SaveChangesAsync();

        return Result.Success();
    }
}