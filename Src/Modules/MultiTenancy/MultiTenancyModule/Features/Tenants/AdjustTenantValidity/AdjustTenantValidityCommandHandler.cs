using MediatR;
using Microsoft.Extensions.Logging;
using MultiTenancyModule.Contract.Services;
using MultiTenancyModule.Features.Tenants.AdjustTenantValidity;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace MultiTenancyModule.Features.Tenants.AdjustTenantValidity
{
    internal sealed class AdjustTenantValidityCommandHandler(
        ITenantService tenantService,
        ILogger<AdjustTenantValidityCommandHandler> logger)
        : ICommandHandler<AdjustTenantValidityCommand, DateTime>
    {
        public async Task<Result<DateTime>> Handle(AdjustTenantValidityCommand request, CancellationToken cancellationToken)
        {
            var validUpto = await tenantService.AdjustValidityAsync(request.Id, request.ValidUpto, cancellationToken).ConfigureAwait(false);

            logger.LogInformation("Tenant validity adjusted: {TenantId}, NewValidUpto: {ValidUpto}", request.Id, validUpto);

            return Result.Success(validUpto);
        }
    }
}