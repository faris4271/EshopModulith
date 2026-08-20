using FluentValidation;
using MultiTenancyModule.Features.Tenants.DeleteTenant;

namespace MultiTenancyModule.Features.Tenants.DeleteTenant
{
    public class DeleteTenantValidator : AbstractValidator<DeleteTenantCommand>
    {
        public DeleteTenantValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Tenant ID is required.");
        }
    }
}