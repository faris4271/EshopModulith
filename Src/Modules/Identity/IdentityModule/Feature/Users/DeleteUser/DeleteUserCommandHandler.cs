using Module.Identity.Contract.Feature.Users.DeleteUser;
using Module.Identity.Contract.Services;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace IdentityModule.Feature.Users.DeleteUser;

public sealed class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand>
{
    private readonly IUserService _userService;

    public DeleteUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        if (request == null)
            return Result.Failure(Error.NullValue);

        await _userService.DeleteAsync(request.Id);
        return Result.Success();
    }
}
