using MultiTenancyModule.Contract.Dtos;
using Shared.Contract.CQRS;

namespace MultiTenancyModule.Features.Tenants.GetTenantById
{
    public record GetTenantByIdQuery(string Id) : IQuery<TenantDto>;
}