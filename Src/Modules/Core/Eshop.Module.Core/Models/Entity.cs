using Shared.DDD;
using System.ComponentModel.DataAnnotations;


namespace Eshop.Module.Core.Models
{
    public class Entity : Aggregate<Guid>,IAuditableEntity
    {
        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(450)]
        public string Slug { get;private set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(450)]
        public string Name { get;private set; }

        public Guid EntityId { get;private set; }

        [StringLength(450)]
        public string EntityTypeId { get;private set; }

        public EntityType EntityType { get;private set; }

        public DateTimeOffset CreatedOn {  get;private set; }

        public string? CreatedById { get;private set; }

        public DateTimeOffset? LatestUpdatedOn {  get;private set; }

        public string? LatestUpdatedById {  get;private set; }

        public static Entity Creat(string name ,string slug, Guid entityId, string entityTypeId)
        {
            Entity entity = new Entity
            {
                Id = Guid.NewGuid(),
                Name = name,
                Slug = slug,
                EntityId = entityId,
                EntityTypeId = entityTypeId,
                

            };
            return entity;
           
        }

        public void Update(string name, string slug, Guid entityId, string entityTypeId)
        {
            ArgumentNullException.ThrowIfNull(name, nameof(name));
            ArgumentNullException.ThrowIfNull(slug, nameof(slug));

            Name = name;
            Slug = slug;
            EntityId = entityId;
            EntityTypeId = entityTypeId;


        }


        public static void AddEntityType(EntityType type)
        {
            var entiy = new EntityType()
            {
                AreaName = type.AreaName,
                CreatedOn = DateTime.UtcNow,
                CreatedById = type.CreatedById,
                


            };



        }
    }
}
