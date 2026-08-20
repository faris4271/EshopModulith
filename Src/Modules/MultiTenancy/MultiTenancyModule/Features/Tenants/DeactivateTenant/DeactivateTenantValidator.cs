using FluentValidation;
using MultiTenancyModule.Features.Tenants.DeactivateTenant;

namespace MultiTenancyModule.Features.Tenants.DeactivateTenant
{
    public class DeactivateTenantValidator : AbstractValidator<DeactivateTenantCommand>
    {
        public DeactivateTenantValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Tenant ID is required.");
        }
    }
}