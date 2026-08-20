using Finbuckle.MultiTenant.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using MultiTenancyModule.Contract.Services;
using MultiTenancyModule.Features.Tenants.SeedTenant;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using Shared.Multitenancy;

namespace MultiTenancyModule.Features.Tenants.SeedTenant
{
    internal sealed class SeedTenantCommandHandler(
        IMultiTenantStore<AppTenantInfo> tenantStore,
        ITenantService tenantService,
        ILogger<SeedTenantCommandHandler> logger)
        : ICommandHandler<SeedTenantCommand>
    {
        public async Task<Result> Handle(SeedTenantCommand request, CancellationToken cancellationToken)
        {
            var tenant = await tenantStore.GetAsync(request.TenantId).ConfigureAwait(false);
            if (tenant is null)
            {
                return Result.Failure(Error.NotFound("Tenant.NotFound", $"Tenant with id '{request.TenantId}' not found."));
            }

            await tenantService.SeedTenantAsync(tenant, cancellationToken).ConfigureAwait(false);

            logger.LogInformation("Tenant seeded: {TenantId}", tenant.Id);

            return Result.Success();
        }
    }
}