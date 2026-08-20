using MediatR;
using MultiTenancyModule.Contract.Dtos;
using MultiTenancyModule.Contract.Services;
using MultiTenancyModule.Features.Tenants.GetTenantById;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace MultiTenancyModule.Features.Tenants.GetTenantById
{
    internal sealed class GetTenantByIdQueryHandler(
        ITenantService tenantService)
        : IQueryHandler<GetTenantByIdQuery, TenantDto>
    {
        public async Task<Result<TenantDto>> Handle(GetTenantByIdQuery request, CancellationToken cancellationToken)
        {
            var tenant = await tenantService.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
            if (tenant is null)
            {
                return Result.Failure<TenantDto>(Error.NotFound("Tenant.NotFound", $"Tenant with id '{request.Id}' not found."));
            }

            var tenantDto = new TenantDto
            {
                Id = tenant.Id,
                Name = tenant.Name,
                ConnectionString = tenant.ConnectionString,
                ValidUpto = tenant.ValidUpto,
                AdminEmail = tenant.AdminEmail,
                IsActive = tenant.IsActive,
                Issuer = tenant.Issuer
            };

            return Result.Success(tenantDto);
        }
    }
}