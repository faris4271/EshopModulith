using Shared.Contract.CQRS;

namespace Module.Identity.Contract.Feature.Sessions.AdminRevokeAllSessions;

public sealed record AdminRevokeAllSessionsCommand(Guid UserId, string? Reason = null) : ICommand<int>;
