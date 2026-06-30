using Shared.DDD;
using Shared.Identity;
using System.ComponentModel.DataAnnotations;


namespace Module.Inventory.Models
{
    public class Warehouse : EntityBase<Guid>
    {


        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(450)]
        public Name Name { get; set; }

        public Guid? VendorId { get; set; }

        public Address Address { get; set; }
    }
}
