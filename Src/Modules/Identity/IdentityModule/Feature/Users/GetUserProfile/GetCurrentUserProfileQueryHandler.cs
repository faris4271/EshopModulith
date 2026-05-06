using Module.Identity.Contract.Dtos;
using Module.Identity.Contract.Feature.Users.GetUserProfile;
using Module.Identity.Contract.Services;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace IdentityModule.Feature.Users.GetUserProfile;

public sealed class GetCurrentUserProfileQueryHandler : IQueryHandler<GetCurrentUserProfileQuery, UserDto>
{
    private readonly IUserService _userService;

    public GetCurrentUserProfileQueryHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Result<UserDto>> Handle(GetCurrentUserProfileQuery request, CancellationToken cancellationToken)
    {
        if (request == null)
            Result.Failure<UserDto>(Error.NullValue);

       
        var result= await _userService.GetAsync(request.UserId, cancellationToken).ConfigureAwait(false);
        return Result.Success(result);


    }
}
