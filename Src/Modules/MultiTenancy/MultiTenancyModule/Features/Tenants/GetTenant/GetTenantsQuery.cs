using MultiTenancyModule.Contract.Dtos;
using Shared.Web.SmartTable;
using Shared.Contract.CQRS;

namespace MultiTenancyModule.Features.Tenants.GetTenant
{
    public record GetTenantsQuery(SmartTableParam SmartTableParam) : IQuery<SmartTableResult<TenantDto>>;
}