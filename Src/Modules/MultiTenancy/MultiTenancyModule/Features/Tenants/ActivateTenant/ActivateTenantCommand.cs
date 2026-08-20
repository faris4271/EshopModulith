using Shared.Contract.CQRS;

namespace MultiTenancyModule.Features.Tenants.ActivateTenant
{
    public record ActivateTenantCommand(string Id) : ICommand<string>;
}