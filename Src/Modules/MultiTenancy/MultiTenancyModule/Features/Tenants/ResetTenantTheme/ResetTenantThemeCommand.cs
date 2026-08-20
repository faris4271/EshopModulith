using Shared.Contract.CQRS;

namespace MultiTenancyModule.Features.Tenants.ResetTenantTheme
{
    public record ResetTenantThemeCommand(string TenantId) : ICommand;
}