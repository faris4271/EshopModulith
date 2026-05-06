using Module.Identity.Contract.Feature.Users.ToggleUserStatus;
using Module.Identity.Contract.Services;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace IdentityModule.Feature.Users.ToggleUserStatus;

public sealed class ToggleUserStatusCommandHandler : ICommandHandler<ToggleUserStatusCommand>
{
    private readonly IUserService _userService;

    public ToggleUserStatusCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Result> Handle(ToggleUserStatusCommand command, CancellationToken cancellationToken)
    {

        if (string.IsNullOrWhiteSpace(command.UserId))
        {
           return Result.Failure(Error.NullValue);
        }

          await _userService.ToggleStatusAsync(command.ActivateUser, command.UserId, cancellationToken).ConfigureAwait(false);

        return Result.Success();
    }
}
