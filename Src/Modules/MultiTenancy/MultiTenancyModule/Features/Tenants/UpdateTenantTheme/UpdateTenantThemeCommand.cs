using MultiTenancyModule.Contract.Dtos;
using Shared.Contract.CQRS;

namespace MultiTenancyModule.Features.Tenants.UpdateTenantTheme
{
    public record UpdateTenantThemeCommand(
        string TenantId,
        TenantThemeDto Theme
    ) : ICommand;
}