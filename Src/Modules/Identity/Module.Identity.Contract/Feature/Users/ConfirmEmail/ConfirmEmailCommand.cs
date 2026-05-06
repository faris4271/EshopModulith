
using Shared.Contract.CQRS;

namespace Module.Identity.Contract.Feature.Users.ConfirmEmail;

public sealed record ConfirmEmailCommand(string UserId, string Code) : ICommand<string>;

