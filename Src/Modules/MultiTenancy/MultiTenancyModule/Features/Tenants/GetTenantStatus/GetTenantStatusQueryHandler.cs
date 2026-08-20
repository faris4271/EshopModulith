using MediatR;
using MultiTenancyModule.Contract.Dtos;
using MultiTenancyModule.Contract.Services;
using MultiTenancyModule.Features.Tenants.GetTenantStatus;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace MultiTenancyModule.Features.Tenants.GetTenantStatus
{
    internal sealed class GetTenantStatusQueryHandler(
        ITenantService tenantService)
        : IQueryHandler<GetTenantStatusQuery, TenantStatusDto>
    {
        public async Task<Result<TenantStatusDto>> Handle(GetTenantStatusQuery request, CancellationToken cancellationToken)
        {
            var exists = await tenantService.ExistsWithIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
            if (!exists)
            {
                return Result.Failure<TenantStatusDto>(Error.NotFound("Tenant.NotFound", $"Tenant with id '{request.Id}' not found."));
            }

            var status = await tenantService.GetStatusAsync(request.Id, cancellationToken).ConfigureAwait(false);
            return Result.Success(status);
        }
    }
}