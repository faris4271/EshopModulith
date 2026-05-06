using Shared.Contract.CQRS;

namespace Module.Identity.Contract.Feature.Sessions.RevokeAllSessions;

public sealed record RevokeAllSessionsCommand(Guid? ExceptSessionId = null) : ICommand<int>;
