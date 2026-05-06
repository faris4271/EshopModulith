using Module.Identity.Contract.Feature.Users.ConfirmEmail;
using Module.Identity.Contract.Services;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace IdentityModule.Feature.Users.ConfirmEmail;

public sealed class ConfirmEmailCommandHandler : ICommandHandler<ConfirmEmailCommand, string>
{
    private readonly IUserService _userService;

    public ConfirmEmailCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Result<string>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        if (request is null)
            return Result.Failure<string>(Error.NullValue);

        var result= await _userService.ConfirmEmailAsync(request.UserId, request.Code, cancellationToken)
            .ConfigureAwait(false);

       return Result.Success(result);
    }
}
