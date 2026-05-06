using FluentValidation;
using Module.Identity.Contract.Feature.Roles.UpsertRole;

namespace IdentityModule.Feature.Roles.UpsertRole;

public sealed class UpsertRoleCommandValidator : AbstractValidator<UpsertRoleCommand>
{
    public UpsertRoleCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Role name is required.");
    }
}