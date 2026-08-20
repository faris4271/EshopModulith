using Shared.Contract.CQRS;

namespace MultiTenancyModule.Features.Tenants.SetDefaultTenantTheme
{
    public record SetDefaultTenantThemeCommand(string TenantId) : ICommand;
}