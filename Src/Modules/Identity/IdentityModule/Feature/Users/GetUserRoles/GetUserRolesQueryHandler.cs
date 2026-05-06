using Module.Identity.Contract.Dtos;
using Module.Identity.Contract.Feature.Users.GetUserRoles;
using Module.Identity.Contract.Services;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace IdentityModule.Feature.Users.GetUserRoles;

public sealed class GetUserRolesQueryHandler : IQueryHandler<GetUserRolesQuery, List<UserRoleDto>>
{
    private readonly IUserService _userService;

    public GetUserRolesQueryHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Result<List<UserRoleDto>>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
    {
        if (request is null)
            return Result.Failure<List<UserRoleDto>>(Error.NullValue);

        var result= await _userService.GetUserRolesAsync(request.UserId, cancellationToken).ConfigureAwait(false);

        return Result.Success(result);
    }
}
