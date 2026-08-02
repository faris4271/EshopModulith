using Shared.Contract.CQRS;

namespace Module.Identity.Contract.Feature.Vendors.DeleteVendor;

public record DeleteVendorCommand(Guid Id) : ICommand<Guid>;