using Shared.DDD;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Products.Models
{
    public class ProductOption : EntityBase<Guid>
    {
        public ProductOption()
        {

        }

        public ProductOption(Guid id)
        {
            Id = id;
        }

        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(450)]
        public string Name { get; set; }
    }
}
