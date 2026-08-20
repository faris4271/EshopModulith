using FluentValidation;
using MultiTenancyModule.Features.Tenants.RenewTenant;

namespace MultiTenancyModule.Features.Tenants.RenewTenant
{
    public class RenewTenantValidator : AbstractValidator<RenewTenantCommand>
    {
        public RenewTenantValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Tenant ID is required.");

            RuleFor(x => x.NewPlanKey)
                .NotEmpty().WithMessage("Plan key is required.")
                .MaximumLength(50).WithMessage("Plan key must not exceed 50 characters.");

            RuleFor(x => x.TermMonths)
                .GreaterThan(0).WithMessage("Term months must be greater than 0.")
                .LessThanOrEqualTo(120).WithMessage("Term months must not exceed 120 (10 years).");
        }
    }
}