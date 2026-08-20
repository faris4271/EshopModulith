using FluentValidation;
using MultiTenancyModule.Features.Tenants.UpdateTenantTheme;

namespace MultiTenancyModule.Features.Tenants.UpdateTenantTheme
{
    public class UpdateTenantThemeValidator : AbstractValidator<UpdateTenantThemeCommand>
    {
        public UpdateTenantThemeValidator()
        {
            RuleFor(x => x.TenantId)
                .NotEmpty().WithMessage("Tenant ID is required.");

            RuleFor(x => x.Theme)
                .NotNull().WithMessage("Theme data is required.");
        }
    }
}