using Module.Identity.Contract.Dtos;
using Module.Identity.Contract.Feature.Sessions.GetMySessions;
using Module.Identity.Contract.Services;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace IdentityModule.Feature.Sessions.GetMySessions;

public sealed class GetMySessionsQueryHandler : IQueryHandler<GetMySessionsQuery, List<UserSessionDto>>
{
    private readonly ISessionService _sessionService;
    private readonly ICurrentUser _currentUser;

    public GetMySessionsQueryHandler(ISessionService sessionService, ICurrentUser currentUser)
    {
        _sessionService = sessionService;
        _currentUser = currentUser;
    }

    public async Task<Result<List<UserSessionDto>>> Handle(GetMySessionsQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.GetUserId().ToString();
        var result= await _sessionService.GetUserSessionsAsync(userId, cancellationToken);

        return Result.Success(result);
    }
}
