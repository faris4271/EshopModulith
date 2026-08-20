using FluentValidation;
using MultiTenancyModule.Contract.Dtos;
using MultiTenancyModule.Features.Tenants.UpdateTenantSettings;

namespace MultiTenancyModule.Features.Tenants.UpdateTenantSettings
{
    public class UpdateTenantSettingsValidator : AbstractValidator<UpdateTenantSettingsCommand>
    {
        public UpdateTenantSettingsValidator()
        {
            RuleFor(x => x.TenantId)
                .NotEmpty().WithMessage("Tenant ID is required.");

            RuleFor(x => x.Settings)
                .NotNull().WithMessage("Settings data is required.");

            // Settings DTO validation is handled by data annotations in the contract
        }
    }
}