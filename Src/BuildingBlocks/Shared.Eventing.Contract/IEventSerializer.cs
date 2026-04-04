using Shared.Message.Events;

namespace Shared.Eventing.Contract;

/// <summary>
/// Serializes and deserializes integration events for transport and storage (outbox).
/// </summary>
public interface IEventSerializer
{
    string Serialize(IntegrationEvent @event);

    IntegrationEvent? Deserialize(string payload, string eventTypeName);
}
