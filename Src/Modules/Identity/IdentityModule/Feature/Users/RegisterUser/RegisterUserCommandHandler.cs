using Module.Identity.Contract.Feature.Users.RegisterUser;
using Module.Identity.Contract.Services;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace IdentityModule.Feature.Users.RegisterUser;

public sealed class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, RegisterUserResponse>
{
    private readonly IUserService _userService;

    public RegisterUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Result<RegisterUserResponse>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        if (command is null)
            return Result.Failure<RegisterUserResponse>(Error.NullValue);

        string userId = await _userService.RegisterAsync(
          command.FirstName,
          command.LastName,
          command.Email,
          command.UserName,
          command.Password,
          command.ConfirmPassword,
          command.PhoneNumber ?? string.Empty,
          command.Origin ?? string.Empty,
          cancellationToken).ConfigureAwait(false);

        return Result.Success(new RegisterUserResponse(userId));
    }
}
