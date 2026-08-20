using Shared.Contract.CQRS;

namespace MultiTenancyModule.Features.Tenants.DeactivateTenant
{
    public record DeactivateTenantCommand(string Id) : ICommand<string>;
}