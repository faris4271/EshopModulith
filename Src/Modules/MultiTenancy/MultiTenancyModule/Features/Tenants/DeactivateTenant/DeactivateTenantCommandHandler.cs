using MediatR;
using Microsoft.Extensions.Logging;
using MultiTenancyModule.Contract.Services;
using MultiTenancyModule.Features.Tenants.DeactivateTenant;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace MultiTenancyModule.Features.Tenants.DeactivateTenant
{
    internal sealed class DeactivateTenantCommandHandler(
        ITenantService tenantService,
        ILogger<DeactivateTenantCommandHandler> logger)
        : ICommandHandler<DeactivateTenantCommand, string>
    {
        public async Task<Result<string>> Handle(DeactivateTenantCommand request, CancellationToken cancellationToken)
        {
            var tenantId = await tenantService.DeactivateAsync(request.Id, cancellationToken).ConfigureAwait(false);

            logger.LogInformation("Tenant deactivated: {TenantId}", tenantId);

            return Result.Success(tenantId);
        }
    }
}