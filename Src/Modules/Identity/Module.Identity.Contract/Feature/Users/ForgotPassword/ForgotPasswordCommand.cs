
using Shared.Contract.CQRS;

namespace Module.Identity.Contract.Feature.Users.ForgotPassword;

public class ForgotPasswordCommand : ICommand<string>
{
    public string Email { get; set; } = default!;
}
