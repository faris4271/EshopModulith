using Module.Identity.Contract.Feature.Users.UpdateUser;
using Module.Identity.Contract.Services;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace IdentityModule.Feature.Users.UpdateUser;

public sealed class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand>
{
    private readonly IUserService _userService;

    public UpdateUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Result> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        if (command is null)
            return Result.Failure(Error.NullValue);

        await _userService.UpdateAsync(
            command.Id,
            command.FirstName ?? string.Empty,
            command.LastName ?? string.Empty,
            command.PhoneNumber ?? string.Empty,
            command.Image!,
            command.DeleteCurrentImage).ConfigureAwait(false);

        return Result.Success();
    }
}
