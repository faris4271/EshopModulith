using EShop.Module.Core.Contract.Services;
using IdentityModule.Data;
using IdentityModule.Domain;
using Module.Identity.Contract.Feature.Vendors;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace IdentityModule.Feature.Vendors.CreatVendors
{
    public class CreatVendorCommandHandler(IGenericeRepository<Vendor, IdentityDbContext> _vendorRepository, IEntityService _entityService) : ICommandHandler<CreatVendorCommand>
    {
        const string EntityType = "Vendor";
        public async Task<Result> Handle(CreatVendorCommand request, CancellationToken cancellationToken)
        {



            var vendor = Vendor.Create(
                request.VendorDto.Name,
                request.VendorDto.Description,
                request.VendorDto.Email,
                request.VendorDto.IsActive);

            var safeSluge = await _entityService.ToSafeSlug(request.VendorDto.Slug, vendor.Id, EntityType);

            await _entityService.Add(vendor.Name.name, safeSluge, vendor.Id, EntityType);

            vendor.AddSafeSlug(safeSluge);

            await _vendorRepository.AddAsync(vendor);

            await _vendorRepository.SaveChangesAsync();


            return Result.Success();
        }
    }
}
