using Shared.Message.Events;

namespace Shared.Eventing.Contract;

/// <summary>
/// Handles a single integration event type.
/// </summary>
/// <typeparam name="TEvent">The integration event type.</typeparam>
public interface IIntegrationEventHandler<in TEvent>
    where TEvent : IntegrationEvent
{
    Task HandleAsync(TEvent @event, CancellationToken ct = default);
}
