namespace Shared.DDD
{
    public interface IEntityBase<T> : IEntityBase
    {
        public T Id { get; set; }
    }

    public interface IEntityBase
    {
        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }
        public string LasteModifiedBy { get; set; }
        public DateTime LasteModified { get; set; }

    }
}
