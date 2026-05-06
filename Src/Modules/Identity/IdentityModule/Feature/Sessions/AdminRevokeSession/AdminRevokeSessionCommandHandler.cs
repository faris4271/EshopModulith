using Module.Identity.Contract.Feature.Sessions.AdminRevokeSession;
using Module.Identity.Contract.Services;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace IdentityModule.Feature.Sessions.AdminRevokeSession;

public sealed class AdminRevokeSessionCommandHandler : ICommandHandler<AdminRevokeSessionCommand,bool>
{
    private readonly ISessionService _sessionService;
    private readonly ICurrentUser _currentUser;

    public AdminRevokeSessionCommandHandler(ISessionService sessionService, ICurrentUser currentUser)
    {
        _sessionService = sessionService;
        _currentUser = currentUser;
    }

    public async Task<Result<bool>> Handle(AdminRevokeSessionCommand command, CancellationToken cancellationToken)
    {
        var adminId = _currentUser.GetUserId().ToString();

        // Get the session to verify it belongs to the specified user
        var session = await _sessionService.GetSessionAsync(command.SessionId, cancellationToken);
        if (session is null || session.UserId != command.UserId.ToString())
        {
            return Result.Failure<bool>(Error.Conflict("401","unkowen errore"));
        }

        // Use the admin revocation method (doesn't check ownership)
        var result= await _sessionService.RevokeSessionForAdminAsync(
            command.SessionId,
            adminId,
            command.Reason ?? "Revoked by administrator",
            cancellationToken);

        return Result.Success(result);
    }
}
