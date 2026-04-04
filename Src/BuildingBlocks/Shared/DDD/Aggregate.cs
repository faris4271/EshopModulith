namespace Shared.DDD
{
    public class Aggregate<T> : EntityBase<T>, IAggregate<T>
    {
        public IReadOnlyList<IDomainEvent> Events => _domainEvents.AsReadOnly();
        private readonly List<IDomainEvent> _domainEvents;

        public IDomainEvent[] ClearDomainEvent()
        {
            var DomainEvent = _domainEvents.ToArray();
            _domainEvents.Clear();
            return DomainEvent;
        }

        public void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }
    }
}
