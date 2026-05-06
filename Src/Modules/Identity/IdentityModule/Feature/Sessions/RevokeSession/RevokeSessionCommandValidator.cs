using FluentValidation;
using Module.Identity.Contract.Feature.Sessions.RevokeSession;

namespace IdentityModule.Feature.Sessions.RevokeSession;

public sealed class RevokeSessionCommandValidator : AbstractValidator<RevokeSessionCommand>
{
    public RevokeSessionCommandValidator()
    {
        RuleFor(x => x.SessionId)
            .NotEmpty().WithMessage("Session ID is required.");
    }
}
