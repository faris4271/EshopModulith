using Module.Identity.Contract.Feature.Sessions.RevokeSession;
using Module.Identity.Contract.Services;
using Shared.Contract.Context;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace IdentityModule.Feature.Sessions.RevokeSession;

public sealed class RevokeSessionCommandHandler : ICommandHandler<RevokeSessionCommand, bool>
{
    private readonly ISessionService _sessionService;
    private readonly ICurrentUser _currentUser;

    public RevokeSessionCommandHandler(ISessionService sessionService, ICurrentUser currentUser)
    {
        _sessionService = sessionService;
        _currentUser = currentUser;
    }

    public async Task<Result<bool>> Handle(RevokeSessionCommand command, CancellationToken cancellationToken)
    {
        var userId = _currentUser.GetUserId().ToString();
        var result = await _sessionService.RevokeSessionAsync(
            command.SessionId,
            userId,
            "User requested",
            cancellationToken);
        return Result.Success(result);
    }
}
