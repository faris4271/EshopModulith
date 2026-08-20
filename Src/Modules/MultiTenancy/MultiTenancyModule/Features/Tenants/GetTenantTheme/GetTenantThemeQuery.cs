using MultiTenancyModule.Contract.Dtos;
using Shared.Contract.CQRS;

namespace MultiTenancyModule.Features.Tenants.GetTenantTheme
{
    public record GetTenantThemeQuery(string TenantId) : IQuery<TenantThemeDto>;

    public record GetCurrentTenantThemeQuery() : IQuery<TenantThemeDto>;

    public record GetDefaultTenantThemeQuery() : IQuery<TenantThemeDto>;
}