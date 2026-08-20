using MediatR;
using Microsoft.Extensions.Logging;
using MultiTenancyModule.Contract.Services;
using MultiTenancyModule.Features.Tenants.RenewTenant;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace MultiTenancyModule.Features.Tenants.RenewTenant
{
    internal sealed class RenewTenantCommandHandler(
        ITenantService tenantService,
        ILogger<RenewTenantCommandHandler> logger)
        : ICommandHandler<RenewTenantCommand, (DateTime PeriodStartUtc, DateTime ValidUpto, bool PlanChanged)>
    {
        public async Task<Result<(DateTime PeriodStartUtc, DateTime ValidUpto, bool PlanChanged)>> Handle(
            RenewTenantCommand request, CancellationToken cancellationToken)
        {
            var result = await tenantService.RenewAsync(request.Id, request.NewPlanKey, request.TermMonths, cancellationToken).ConfigureAwait(false);

            logger.LogInformation("Tenant renewed: {TenantId}, PlanChanged: {PlanChanged}, NewValidUpto: {ValidUpto}",
                request.Id, result.PlanChanged, result.ValidUpto);

            return Result.Success(result);
        }
    }
}