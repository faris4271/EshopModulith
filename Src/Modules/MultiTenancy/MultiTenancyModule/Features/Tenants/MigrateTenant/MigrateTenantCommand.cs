using Shared.Contract.CQRS;

namespace MultiTenancyModule.Features.Tenants.MigrateTenant
{
    public record MigrateTenantCommand(string TenantId) : ICommand;
}