using Module.Identity.Contract.Dtos;
using Module.Identity.Contract.Feature.Users.GetUser;
using Module.Identity.Contract.Services;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace IdentityModule.Feature.Users.GetUserById;

public sealed class GetUserByIdQueryHandler : IQueryHandler<GetUserQuery, UserDto>
{
    private readonly IUserService _userService;

    public GetUserByIdQueryHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Result<UserDto>> Handle(GetUserQuery query, CancellationToken cancellationToken)
    {
        if (query is null)
            return Result.Failure<UserDto>(Error.NullValue);

        var result= await _userService.GetAsync(query.Id, cancellationToken).ConfigureAwait(false);

        return Result.Success(result);
    }
}
