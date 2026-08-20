using Microsoft.Extensions.Logging;
using MultiTenancyModule.Contract.Services;
using MultiTenancyModule.Provisioning;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace MultiTenancyModule.Features.Tenants.CreateTenant
{
    internal sealed class CreateTenantCommandHandler(
        ITenantService tenantService,
        ITenantProvisioningService provisioningService,
        ILogger<CreateTenantCommandHandler> logger)
        : ICommandHandler<CreateTenantCommand, string>
    {
        public async Task<Result<string>> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
        {
            // Check identifier uniqueness
            if (await tenantService.ExistsWithIdAsync(request.Identifier, cancellationToken).ConfigureAwait(false))
            {
                return Result.Failure<string>(Error.Conflict("Tenant.IdentifierExists", $"Tenant with identifier '{request.Identifier}' already exists."));
            }

            // Check name uniqueness
            if (await tenantService.ExistsWithNameAsync(request.Name, cancellationToken).ConfigureAwait(false))
            {
                return Result.Failure<string>(Error.Conflict("Tenant.NameExists", $"Tenant with name '{request.Name}' already exists."));
            }

            // Create tenant via service (handles store + cache)
            var tenantId = await tenantService.CreateAsync(
                request.Identifier,
                request.Name,
                request.ConnectionString,
                request.AdminEmail,
                request.Issuer,
                request.PlanKey,
                request.ValidUpto,
                cancellationToken).ConfigureAwait(false);

            // Get the created tenant for provisioning
            var tenant = await tenantService.GetByIdAsync(tenantId, cancellationToken).ConfigureAwait(false);
            if (tenant is not null)
            {
                await provisioningService.StartAsync(tenant.Id, cancellationToken).ConfigureAwait(false);
            }

            logger.LogInformation("Tenant created: {TenantId}", tenantId);

            return Result.Success(tenantId);
        }
    }
}