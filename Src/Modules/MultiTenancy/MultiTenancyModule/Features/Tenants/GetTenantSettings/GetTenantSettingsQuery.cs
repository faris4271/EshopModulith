using MultiTenancyModule.Contract.Dtos;
using Shared.Contract.CQRS;

namespace MultiTenancyModule.Features.Tenants.GetTenantSettings
{
    public record GetTenantSettingsQuery(Guid TenantId) : IQuery<TenantSettingsDto>;
}