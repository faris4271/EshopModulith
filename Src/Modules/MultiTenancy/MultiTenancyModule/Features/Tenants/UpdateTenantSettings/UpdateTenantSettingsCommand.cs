using MultiTenancyModule.Contract.Dtos;
using Shared.Contract.CQRS;

namespace MultiTenancyModule.Features.Tenants.UpdateTenantSettings
{
    public record UpdateTenantSettingsCommand(
        Guid TenantId,
        UpdateTenantSettingsDto Settings
    ) : ICommand<TenantSettingsDto>;
}