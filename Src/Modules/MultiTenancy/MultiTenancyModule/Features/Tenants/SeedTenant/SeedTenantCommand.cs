using Shared.Contract.CQRS;

namespace MultiTenancyModule.Features.Tenants.SeedTenant
{
    public record SeedTenantCommand(string TenantId) : ICommand;
}