using Shared.Message.Events;

namespace Module.Identity.Contract.Events
{
    public sealed record UserRegisteredIntegrationEvent(
       Guid Id,
       string Source,
       string UserId,
       string Email,
       string FirstName,
       string LastName)
       : IntegrationEvent;
}
