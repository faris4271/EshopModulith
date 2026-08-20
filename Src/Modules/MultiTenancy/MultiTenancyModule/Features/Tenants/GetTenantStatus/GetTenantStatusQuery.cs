using MultiTenancyModule.Contract.Dtos;
using Shared.Contract.CQRS;

namespace MultiTenancyModule.Features.Tenants.GetTenantStatus
{
    public record GetTenantStatusQuery(string Id) : IQuery<TenantStatusDto>;
}