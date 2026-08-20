using Finbuckle.MultiTenant.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using MultiTenancyModule.Contract.Services;
using MultiTenancyModule.Features.Tenants.MigrateTenant;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using Shared.Multitenancy;

namespace MultiTenancyModule.Features.Tenants.MigrateTenant
{
    internal sealed class MigrateTenantCommandHandler(
        IMultiTenantStore<AppTenantInfo> tenantStore,
        ITenantService tenantService,
        ILogger<MigrateTenantCommandHandler> logger)
        : ICommandHandler<MigrateTenantCommand>
    {
        public async Task<Result> Handle(MigrateTenantCommand request, CancellationToken cancellationToken)
        {
            var tenant = await tenantStore.GetAsync(request.TenantId).ConfigureAwait(false);
            if (tenant is null)
            {
                return Result.Failure(Error.NotFound("Tenant.NotFound", $"Tenant with id '{request.TenantId}' not found."));
            }

            await tenantService.MigrateTenantAsync(tenant, cancellationToken).ConfigureAwait(false);

            logger.LogInformation("Tenant migrated: {TenantId}", tenant.Id);

            return Result.Success();
        }
    }
}