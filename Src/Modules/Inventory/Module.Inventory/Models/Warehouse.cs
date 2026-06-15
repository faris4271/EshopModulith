using Shared.DDD;
using Shared.Identity;
using System.ComponentModel.DataAnnotations;


namespace Module.Inventory.Models
{
    public class Warehouse : EntityBase<Guid>
    {


        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(450)]
        public string Name { get; set; }

        public long AddressId { get; set; }

        public long? VendorId { get; set; }

        public Address Address { get; set; }
    }
}
