using FluentValidation;
using MultiTenancyModule.Features.Tenants.ActivateTenant;

namespace MultiTenancyModule.Features.Tenants.ActivateTenant
{
    public class ActivateTenantValidator : AbstractValidator<ActivateTenantCommand>
    {
        public ActivateTenantValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Tenant ID is required.");
        }
    }
}