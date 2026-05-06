using Shared.Contract.CQRS;

namespace Module.Identity.Contract.Feature.Sessions.RevokeSession;

public sealed record RevokeSessionCommand(Guid SessionId) : ICommand<bool>;
