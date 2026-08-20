using Finbuckle.MultiTenant.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using MultiTenancyModule.Contract.Services;
using MultiTenancyModule.Features.Tenants.DeleteTenant;
using MultiTenancyModule.Provisioning;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using Shared.Multitenancy;

namespace MultiTenancyModule.Features.Tenants.DeleteTenant
{
    internal sealed class DeleteTenantCommandHandler(
        IMultiTenantStore<AppTenantInfo> tenantStore,
        ITenantProvisioningService provisioningService,
        ILogger<DeleteTenantCommandHandler> logger)
        : ICommandHandler<DeleteTenantCommand, string>
    {
        public async Task<Result<string>> Handle(DeleteTenantCommand request, CancellationToken cancellationToken)
        {
            var tenant = await tenantStore.GetAsync(request.Id).ConfigureAwait(false);
            if (tenant is null)
            {
                return Result.Failure<string>(Error.NotFound("Tenant.NotFound", $"Tenant with id '{request.Id}' not found."));
            }

            if (tenant.Id.Equals(MultitenancyConstants.Root.Id, StringComparison.OrdinalIgnoreCase))
            {
                return Result.Failure<string>(Error.Forbidden("Tenant.RootDeletionForbidden", "The root tenant cannot be deleted."));
            }

            var allTenants = await tenantStore.GetAllAsync().ConfigureAwait(false);
            var activeCount = allTenants.Count(t => t.IsActive);

            if (activeCount <= 1 && tenant.IsActive)
            {
                return Result.Failure<string>(Error.Forbidden("Tenant.LastActiveDeletionForbidden", "Cannot delete the last active tenant."));
            }

            // Deprovision the tenant first
            // await provisioningService.DeprovisionAsync(tenant.Id, cancellationToken).ConfigureAwait(false);

            // Then remove from store
            await tenantStore.RemoveAsync(request.Id).ConfigureAwait(false);

            logger.LogInformation("Tenant deleted: {TenantId}", tenant.Id);

            return Result.Success(tenant.Id);
        }
    }
}