using Shared.DDD;
using System.ComponentModel.DataAnnotations;

namespace Eshop.Module.Localization.Models
{
    public class LocalizedContentProperty : EntityBase<Guid>
    {
        public Guid EntityId { get; set; }

        [StringLength(450)]
        public string EntityType { get; set; }

        [Required]
        public string CultureId { get; set; }

        public Culture Culture { get; set; }

        [Required]
        [StringLength(450)]
        public string ProperyName { get; set; }

        public string Value { get; set; }
    }
}
