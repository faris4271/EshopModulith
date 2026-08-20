using FluentValidation;
using MultiTenancyModule.Features.Tenants.CreateTenant;

namespace MultiTenancyModule.Features.Tenants.CreateTenant
{
    public class CreateTenantValidator : AbstractValidator<CreateTenantCommand>
    {
        public CreateTenantValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tenant name is required.")
                .MaximumLength(100).WithMessage("Tenant name must not exceed 100 characters.");

            RuleFor(x => x.Identifier)
                .NotEmpty().WithMessage("Tenant identifier is required.")
                .MaximumLength(100).WithMessage("Tenant identifier must not exceed 100 characters.")
                .Matches("^[a-z0-9-]+$").WithMessage("Tenant identifier must contain only lowercase letters, numbers, and hyphens.");

            RuleFor(x => x.AdminEmail)
                .NotEmpty().WithMessage("Admin email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(200).WithMessage("Admin email must not exceed 200 characters.");

            RuleFor(x => x.PlanKey)
                .NotEmpty().WithMessage("Plan key is required.")
                .MaximumLength(50).WithMessage("Plan key must not exceed 50 characters.");

            RuleFor(x => x.ValidUpto)
                .NotEmpty().WithMessage("Validity date is required.")
                .GreaterThan(DateTime.UtcNow).WithMessage("Validity date must be in the future.");

            RuleFor(x => x.ConnectionString)
                .MaximumLength(500).WithMessage("Connection string must not exceed 500 characters.")
                .When(x => !string.IsNullOrEmpty(x.ConnectionString));

            RuleFor(x => x.Issuer)
                .MaximumLength(200).WithMessage("Issuer must not exceed 200 characters.")
                .When(x => !string.IsNullOrEmpty(x.Issuer));
        }
    }
}