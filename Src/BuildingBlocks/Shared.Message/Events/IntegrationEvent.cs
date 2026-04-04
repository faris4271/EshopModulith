namespace Shared.Message.Events
{
    public record IntegrationEvent
    {
        public Guid Id => Guid.NewGuid();
        public DateTime OcurredOn => DateTime.UtcNow;

        public string EventType => GetType().AssemblyQualifiedName;
    }
}
