using Shared.Contract.CQRS;

namespace MultiTenancyModule.Features.Tenants.CreateTenant
{
    public record CreateTenantCommand(
        string Name,
        string Identifier,
        string? ConnectionString,
        string AdminEmail,
        string Issuer,
        string PlanKey,
        DateTime ValidUpto
    ) : ICommand<string>;
}