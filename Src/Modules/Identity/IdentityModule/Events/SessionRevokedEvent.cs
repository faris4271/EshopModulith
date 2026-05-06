using Shared.DDD;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityModule.Events
{
    public record SessionRevokedEvent(
    Guid eventId ,
    DateTimeOffset OccurredOn,
    string UserId,
    Guid SessionId,
    string? RevokedBy,
    string? Reason,
    string? CorrelationId = null,
    string? TenantId = null
        ) :DomainEvent(eventId,OccurredOn)
    {

        public static SessionRevokedEvent Create(string userId, Guid sessionId, string? revokedBy = null, string? reason = null, string? correlationId = null)
    => new(Guid.NewGuid(), DateTimeOffset.UtcNow, userId, sessionId, revokedBy, reason, correlationId);

    }
}
