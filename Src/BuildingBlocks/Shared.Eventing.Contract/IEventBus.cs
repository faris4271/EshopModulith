using Shared.Message.Events;

namespace Shared.Eventing.Contract;

/// <summary>
/// Abstraction over an event bus. The initial provider is in-memory; additional providers
/// can be added without changing modules that publish or handle events.
/// </summary>
public interface IEventBus
{
    Task PublishAsync(IntegrationEvent @event, CancellationToken ct = default);

    Task PublishAsync(IEnumerable<IntegrationEvent> events, CancellationToken ct = default);
}
