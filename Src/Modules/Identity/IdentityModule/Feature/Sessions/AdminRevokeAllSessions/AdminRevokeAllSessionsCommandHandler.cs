using Module.Identity.Contract.Feature.Sessions.AdminRevokeAllSessions;
using Module.Identity.Contract.Services;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace IdentityModule.Feature.Sessions.AdminRevokeAllSessions;

public sealed class AdminRevokeAllSessionsCommandHandler : ICommandHandler<AdminRevokeAllSessionsCommand, int>
{
    private readonly ISessionService _sessionService;
    private readonly ICurrentUser _currentUser;

    public AdminRevokeAllSessionsCommandHandler(ISessionService sessionService, ICurrentUser currentUser)
    {
        _sessionService = sessionService;
        _currentUser = currentUser;
    }

    public async Task<Result<int>> Handle(AdminRevokeAllSessionsCommand command, CancellationToken cancellationToken)
    {
        var adminId = _currentUser.GetUserId().ToString();
        var result= await _sessionService.RevokeAllSessionsForAdminAsync(
            command.UserId.ToString(),
            adminId,
            command.Reason ?? "Revoked by administrator",
            cancellationToken);

        return Result.Success(result);
    }
}
