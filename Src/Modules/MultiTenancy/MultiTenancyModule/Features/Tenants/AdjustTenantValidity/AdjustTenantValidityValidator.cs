using FluentValidation;
using MultiTenancyModule.Features.Tenants.AdjustTenantValidity;

namespace MultiTenancyModule.Features.Tenants.AdjustTenantValidity
{
    public class AdjustTenantValidityValidator : AbstractValidator<AdjustTenantValidityCommand>
    {
        public AdjustTenantValidityValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Tenant ID is required.");

            RuleFor(x => x.ValidUpto)
                .NotEmpty().WithMessage("Validity date is required.");
        }
    }
}