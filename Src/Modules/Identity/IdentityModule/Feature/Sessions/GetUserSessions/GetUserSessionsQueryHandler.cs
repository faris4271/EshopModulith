using Module.Identity.Contract.Dtos;
using Module.Identity.Contract.Feature.Sessions.GetUserSessions;
using Module.Identity.Contract.Services;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace IdentityModule.Feature.Sessions.GetUserSessions;

public sealed class GetUserSessionsQueryHandler : IQueryHandler<GetUserSessionsQuery, List<UserSessionDto>>
{
    private readonly ISessionService _sessionService;

    public GetUserSessionsQueryHandler(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    public async Task<Result<List<UserSessionDto>>> Handle(GetUserSessionsQuery request, CancellationToken cancellationToken)
    {
       var result = await _sessionService
            .GetUserSessionsForAdminAsync(request.UserId.ToString(), cancellationToken);

        return Result.Success(result);
    }
}
