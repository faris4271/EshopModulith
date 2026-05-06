using FluentValidation;
using Module.Identity.Contract.Feature.Sessions.RevokeAllSessions;

namespace IdentityModule.Feature.Sessions.RevokeAllSessions;

public sealed class RevokeAllSessionsCommandValidator : AbstractValidator<RevokeAllSessionsCommand>
{
    public RevokeAllSessionsCommandValidator()
    {
        // ExceptSessionId is optional - no validation required
        // This validator exists for consistency and potential future validation rules
    }
}
