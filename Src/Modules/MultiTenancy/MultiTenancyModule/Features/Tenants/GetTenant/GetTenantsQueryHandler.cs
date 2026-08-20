using MediatR;
using Microsoft.EntityFrameworkCore;
using MultiTenancyModule.Contract.Dtos;
using MultiTenancyModule.Contract.Services;
using MultiTenancyModule.Data;
using MultiTenancyModule.Features.Tenants.GetTenant;
using Shared.Web.SmartTable;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace MultiTenancyModule.Features.Tenants.GetTenant
{
    internal sealed class GetTenantsQueryHandler(
        ITenantService tenantService)
        : IQueryHandler<GetTenantsQuery, SmartTableResult<TenantDto>>
    {
        public async Task<Result<SmartTableResult<TenantDto>>> Handle(GetTenantsQuery request, CancellationToken cancellationToken)
        {
            var result = await tenantService.GetAllAsync(request.SmartTableParam, cancellationToken).ConfigureAwait(false);
            return Result.Success(result);
        }
    }
}