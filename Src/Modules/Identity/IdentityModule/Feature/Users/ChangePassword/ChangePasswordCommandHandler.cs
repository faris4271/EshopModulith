

using Module.Identity.Contract.Feature.Users.ChangePassword;
using Module.Identity.Contract.Services;
using SendGrid.Helpers.Errors.Model;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace IdentityModule.Feature.Users.ChangePassword;

public sealed class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand, string>
{
    private readonly IUserService _userService;
    private readonly ICurrentUser _currentUser;

    public ChangePasswordCommandHandler(IUserService userService, ICurrentUser currentUser)
    {
        _userService = userService;
        _currentUser = currentUser;
    }

    public async Task<Result<string>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        if (!_currentUser.IsAuthenticated())
            return Result.Failure<string>(Error.Unauthorized("401", "you not loged in"));
        var userId = _currentUser.GetUserId();
       await _userService.ChangePasswordAsync(
           request.Password, request.NewPassword,
           request.ConfirmNewPassword, userId.ToString());

        return Result.Success("password changed success");


    }
}
