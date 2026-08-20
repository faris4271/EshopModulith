using FluentValidation;
using MultiTenancyModule.Features.Tenants.MigrateTenant;

namespace MultiTenancyModule.Features.Tenants.MigrateTenant
{
    public class MigrateTenantValidator : AbstractValidator<MigrateTenantCommand>
    {
        public MigrateTenantValidator()
        {
            RuleFor(x => x.TenantId)
                .NotEmpty().WithMessage("Tenant ID is required.");
        }
    }
}