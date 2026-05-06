using Shared.Contract.CQRS;

namespace Module.Identity.Contract.Feature.Roles.DeleteRole;

public sealed record DeleteRoleCommand(string Id) : ICommand;

