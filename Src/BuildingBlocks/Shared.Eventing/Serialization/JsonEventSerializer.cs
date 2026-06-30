using Shared.Eventing.Contract;
using System.Text.Json;

namespace Shared.Eventing.Serialization
{
    internal class JsonEventSerializer : IEventSerializer
    {
        private static readonly JsonSerializerOptions Options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
        };

        public string Serialize(IIntegrationEvent @event)
        {
            ArgumentNullException.ThrowIfNull(@event);

            return JsonSerializer.Serialize(@event, @event.GetType(), Options);
        }

        public IIntegrationEvent? Deserialize(string payload, string eventTypeName)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(payload);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(eventTypeName);

            var type = Type.GetType(@eventTypeName, throwOnError: false);
            if (type == null)
                return null;

            var result = JsonSerializer.Deserialize(payload, type, Options);

            return result as IIntegrationEvent;
        }
    }
}