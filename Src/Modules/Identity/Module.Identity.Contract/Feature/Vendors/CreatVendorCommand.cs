using Module.Identity.Contract.Dtos;
using Shared.Contract.CQRS;

namespace Module.Identity.Contract.Feature.Vendors
{
    public record CreatVendorCommand(CreatVendorDto VendorDto) : ICommand;


}
