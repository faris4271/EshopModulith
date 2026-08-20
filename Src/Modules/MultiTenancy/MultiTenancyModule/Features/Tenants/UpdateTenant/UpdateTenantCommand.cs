using MultiTenancyModule.Contract.Dtos;
using Shared.Contract.CQRS;

namespace MultiTenancyModule.Features.Tenants.UpdateTenant
{
    public record UpdateTenantCommand(
        string Id,
        string? Name,
        string? ConnectionString,
        string? SubscriptionPlan,
        DateTime? TrialEndsAt,
        DateTime? SubscriptionEndsAt
    ) : ICommand<string>;
}