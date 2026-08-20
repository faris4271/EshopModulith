using Shared.Contract.CQRS;

namespace MultiTenancyModule.Features.Tenants.DeleteTenant
{
    public record DeleteTenantCommand(string Id) : ICommand<string>;
}