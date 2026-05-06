namespace Shared.DDD
{
    public interface IEntityBase<out TId> 
    {
        TId Id { get; }
    }


}
