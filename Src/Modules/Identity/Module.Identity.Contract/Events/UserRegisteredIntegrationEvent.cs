using Shared.Eventing.Contract;

namespace Module.Identity.Contract.Events
{
    public sealed record UserRegisteredIntegrationEvent(
       Guid Id,
       DateTime OccurredOnUtc,
       string CorrelationId,
       string Source,
       string UserId,
       string Email,
       string FirstName,
       string LastName)
       : IIntegrationEvent;

}
