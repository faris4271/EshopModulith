using FluentValidation;
using MultiTenancyModule.Features.Tenants.UpdateTenant;

namespace MultiTenancyModule.Features.Tenants.UpdateTenant
{
    public class UpdateTenantValidator : AbstractValidator<UpdateTenantCommand>
    {
        public UpdateTenantValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Tenant ID is required.");

            RuleFor(x => x.Name)
                .MaximumLength(100).WithMessage("Tenant name must not exceed 100 characters.")
                .When(x => !string.IsNullOrEmpty(x.Name));

            RuleFor(x => x.ConnectionString)
                .MaximumLength(500).WithMessage("Connection string must not exceed 500 characters.")
                .When(x => !string.IsNullOrEmpty(x.ConnectionString));

            RuleFor(x => x.SubscriptionPlan)
                .MaximumLength(50).WithMessage("Subscription plan must not exceed 50 characters.")
                .When(x => !string.IsNullOrEmpty(x.SubscriptionPlan));
        }
    }
}