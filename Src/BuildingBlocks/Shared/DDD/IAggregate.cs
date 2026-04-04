namespace Shared.DDD
{
    public interface IAggregate<T> : IAggregate, IEntityBase<T>
    {
    }
    public interface IAggregate : IEntityBase
    {
        IReadOnlyList<IDomainEvent> Events { get; }
        IDomainEvent[] ClearDomainEvent();
    }
}
