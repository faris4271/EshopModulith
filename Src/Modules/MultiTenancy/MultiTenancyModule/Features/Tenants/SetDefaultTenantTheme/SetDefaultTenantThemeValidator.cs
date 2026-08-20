using FluentValidation;
using MultiTenancyModule.Features.Tenants.SetDefaultTenantTheme;

namespace MultiTenancyModule.Features.Tenants.SetDefaultTenantTheme
{
    public class SetDefaultTenantThemeValidator : AbstractValidator<SetDefaultTenantThemeCommand>
    {
        public SetDefaultTenantThemeValidator()
        {
            RuleFor(x => x.TenantId)
                .NotEmpty().WithMessage("Tenant ID is required.");
        }
    }
}