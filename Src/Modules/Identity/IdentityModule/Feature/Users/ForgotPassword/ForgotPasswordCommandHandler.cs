
using Module.Identity.Contract.Services;
using Shared.Contract.CQRS;
using Module.Identity.Contract.Feature.Users.ForgotPassword;
using Shared.Web.Origin;
using Microsoft.Extensions.Options;
using Shared.Contract.ResultPattern;

namespace IdentityModule.Feature.Users.ForgotPassword;

public sealed class ForgotPasswordCommandHandler : ICommandHandler<ForgotPasswordCommand, string>
{
    private readonly IUserService _userService;
    private readonly IOptions<OriginOptions> _originOptions;

    public ForgotPasswordCommandHandler(IUserService userService, IOptions<OriginOptions> originOptions)
    {
        _userService = userService;
        _originOptions = originOptions;
    }

    public async Task<Result<string>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var origin = _originOptions.Value?.OriginUrl?.ToString();
        if (string.IsNullOrWhiteSpace(origin))
        {
            return Result.Failure<string>(Error.NullValue);
        }

        await _userService.ForgotPasswordAsync(request.Email, origin, cancellationToken).ConfigureAwait(false);

        return Result.Success("Email sended");
    }
}
