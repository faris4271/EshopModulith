using Shared.Contract.CQRS;

namespace MultiTenancyModule.Features.Tenants.RenewTenant
{
    public record RenewTenantCommand(
        string Id,
        string NewPlanKey,
        int TermMonths
    ) : ICommand<(DateTime PeriodStartUtc, DateTime ValidUpto, bool PlanChanged)>;
}