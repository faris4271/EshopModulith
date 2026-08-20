using FluentValidation;
using MultiTenancyModule.Features.Tenants.SeedTenant;

namespace MultiTenancyModule.Features.Tenants.SeedTenant
{
    public class SeedTenantValidator : AbstractValidator<SeedTenantCommand>
    {
        public SeedTenantValidator()
        {
            RuleFor(x => x.TenantId)
                .NotEmpty().WithMessage("Tenant ID is required.");
        }
    }
}