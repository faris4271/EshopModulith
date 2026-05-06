using FluentValidation;
using Module.Identity.Contract.Feature.Roles.DeleteRole;

namespace IdentityModule.Feature.Roles.DeleteRole;

public sealed class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
{
    public DeleteRoleCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Role ID is required.");
    }
}
