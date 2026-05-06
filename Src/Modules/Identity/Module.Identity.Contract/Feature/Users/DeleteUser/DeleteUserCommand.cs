using Shared.Contract.CQRS;

namespace Module.Identity.Contract.Feature.Users.DeleteUser;

public sealed record DeleteUserCommand(string Id) : ICommand;

