using Module.Identity.Contract.Feature.Users.GetUserPermissions;
using Module.Identity.Contract.Services;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace IdentityModule.Feature.Users.GetUserPermissions;

public sealed class GetCurrentUserPermissionsQueryHandler : IQueryHandler<GetCurrentUserPermissionsQuery, List<string>?>
{
    private readonly IUserService _userService;

    public GetCurrentUserPermissionsQueryHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Result<List<string>?>> Handle(GetCurrentUserPermissionsQuery request, CancellationToken cancellationToken)
    {
        return Result.Failure<List<string>>(Error.NullValue);


        var result= await _userService.GetPermissionsAsync(
            request.UserId, cancellationToken).ConfigureAwait(false);

        return Result.Success(result);
    }
}
