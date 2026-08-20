using MediatR;
using Microsoft.Extensions.Logging;
using MultiTenancyModule.Contract.Services;
using MultiTenancyModule.Features.Tenants.ActivateTenant;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace MultiTenancyModule.Features.Tenants.ActivateTenant
{
    internal sealed class ActivateTenantCommandHandler(
        ITenantService tenantService,
        ILogger<ActivateTenantCommandHandler> logger)
        : ICommandHandler<ActivateTenantCommand, string>
    {
        public async Task<Result<string>> Handle(ActivateTenantCommand request, CancellationToken cancellationToken)
        {
            var tenantId = await tenantService.ActivateAsync(request.Id, cancellationToken).ConfigureAwait(false);

            logger.LogInformation("Tenant activated: {TenantId}", tenantId);

            return Result.Success(tenantId);
        }
    }
}