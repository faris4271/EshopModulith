using Shared.DDD;
using System.ComponentModel.DataAnnotations;

namespace Eshop.Module.Core.Models
{
    public class WidgetZone : EntityBase<Guid>
    {

        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(450)]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
