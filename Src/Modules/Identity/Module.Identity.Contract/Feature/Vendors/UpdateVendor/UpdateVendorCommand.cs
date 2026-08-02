using Module.Identity.Contract.Dtos;
using Shared.Contract.CQRS;

namespace Module.Identity.Contract.Feature.Vendors.UpdateVendor;

public record UpdateVendorCommand(Guid Id, CreatVendorDto VendorDto) : ICommand;