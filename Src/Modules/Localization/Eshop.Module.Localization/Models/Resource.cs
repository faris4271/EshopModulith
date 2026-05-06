using Shared.DDD;
using System.ComponentModel.DataAnnotations;

namespace Eshop.Module.Localization.Models
{
    public class Resource : EntityBase<string>
    {
        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(450)]
        public string Key { get; set; }

        public string Value { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        public string CultureId { get; set; }

        public Culture Culture { get; set; }
    }
}