using IdentityModule.Data;
using IdentityModule.Domain;
using Module.Identity.Contract.Dtos;
using Module.Identity.Contract.Feature.Vendors;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace IdentityModule.Feature.Vendors.GetVendorById
{
    internal class GetVendorByIdQueryHandler(IGenericeRepository<Vendor, IdentityDbContext> _vendorRepository) : IQueryHandler<GetVendorByIdQuery, VendorDto>
    {
        public async Task<Result<VendorDto>> Handle(GetVendorByIdQuery request, CancellationToken cancellationToken)
        {
            var vendor = await _vendorRepository.GetByIdAsync(request.Id);

            if (vendor == null)
                return Result.Failure<VendorDto>(Error.NullValue);

            var result = new VendorDto
            (
                 vendor.Id,
                 vendor.Name.name,
                 vendor.Slug,
                 vendor.Email.email,
                 vendor.Description.description,
                 vendor.IsActive
               );


            return Result.Success(result);

        }
    }
}
