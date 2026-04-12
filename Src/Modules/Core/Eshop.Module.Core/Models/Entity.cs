using Shared.DDD;
using System.ComponentModel.DataAnnotations;

namespace Eshop.Module.Core.Models
{
    public class Entity : Aggregate<Guid>
    {
        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(450)]
        public string Slug { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(450)]
        public string Name { get; set; }

        public Guid EntityId { get; set; }

        [StringLength(450)]
        public string EntityTypeId { get; set; }

        public EntityType EntityType { get; set; }


        public static void AddEntityType(EntityType type)
        {
            var entiy = new EntityType()
            {
                AreaName = type.AreaName,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = type.CreatedBy,
                Id = type.Id,


            };



        }
    }
}
