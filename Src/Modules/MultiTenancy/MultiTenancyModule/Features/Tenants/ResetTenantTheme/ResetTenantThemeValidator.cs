using FluentValidation;
using MultiTenancyModule.Features.Tenants.ResetTenantTheme;

namespace MultiTenancyModule.Features.Tenants.ResetTenantTheme
{
    public class ResetTenantThemeValidator : AbstractValidator<ResetTenantThemeCommand>
    {
        public ResetTenantThemeValidator()
        {
            RuleFor(x => x.TenantId)
                .NotEmpty().WithMessage("Tenant ID is required.");
        }
    }
}