namespace Shared.DDD
{
    public class EntityBase<T> : IEntityBase<T>
    {
        public T Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string LasteModifiedBy { get; set; }
        public DateTime LasteModified { get; set; }
    }
}
