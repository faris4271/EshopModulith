using Shared.Contract.CQRS;

namespace Module.Identity.Contract.Feature.Sessions.AdminRevokeSession;

public sealed record AdminRevokeSessionCommand(Guid UserId, Guid SessionId, string? Reason = null) : ICommand<bool>;
