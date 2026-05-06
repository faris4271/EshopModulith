using Shared.DDD;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;



namespace Eshop.Module.Localization.Models
{
    public class Culture : EntityBase<string>
    {
        public Culture(string id)
        {
            Id = id;
        }

        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(450)]
        public string Name { get; set; }

        public IList<Resource> Resources { get; set; }
    }
}