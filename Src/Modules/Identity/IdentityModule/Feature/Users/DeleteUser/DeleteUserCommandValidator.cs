using FluentValidation;
using Module.Identity.Contract.Feature.Users.DeleteUser;

namespace IdentityModule.Feature.Users.DeleteUser;

public sealed class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("User ID is required.");
    }
}
