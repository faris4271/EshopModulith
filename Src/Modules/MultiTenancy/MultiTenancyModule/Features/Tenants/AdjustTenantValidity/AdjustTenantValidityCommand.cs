using Shared.Contract.CQRS;

namespace MultiTenancyModule.Features.Tenants.AdjustTenantValidity
{
    public record AdjustTenantValidityCommand(
        string Id,
        DateTime ValidUpto
    ) : ICommand<DateTime>;
}