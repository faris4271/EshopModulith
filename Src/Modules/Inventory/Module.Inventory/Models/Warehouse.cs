using Shared.DDD;
using System.ComponentModel.DataAnnotations;


namespace Module.Inventory.Models
{
    public class Warehouse : EntityBase<Guid>
    {
        public Warehouse() { }

        public Warehouse(Guid id, Name name, Guid? vendorId, Guid addressId)
        {
            Id = id;
            Name = name;
            VendorId = vendorId;
            AddressId = addressId;
        }

        public static Warehouse Create(string name, Guid? vendorId, Guid addressId)
        {
            return new Warehouse(
                Guid.NewGuid(),
                new Name(name),
                vendorId,
                Guid.NewGuid()
            );
        }

        public void Update(string name, Guid? vendorId, Guid addressId)
        {
            Name = new Name(name);
            VendorId = vendorId;
            AddressId = addressId;
        }

        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(450)]
        public Name Name { get; set; }

        public Guid? VendorId { get; set; }

        public Guid AddressId { get; set; }
    }
}
