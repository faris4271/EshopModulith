using Shared.DDD;
using Shared.Identity;
using System.ComponentModel.DataAnnotations;


namespace Module.Inventory.Models
{
    public class Warehouse : EntityBase<Guid>
    {
        public Warehouse() { }

        public Warehouse(Guid id, Name name, Guid? vendorId, Address address)
        {
            Id = id;
            Name = name;
            VendorId = vendorId;
            Address = address;
        }

        public static Warehouse Create(string name, Guid? vendorId, string street, string city, string state, string zipCode, string country, string phone, string postalCode)
        {
            return new Warehouse(
                Guid.NewGuid(),
                new Name(name),
                vendorId,
                new Address(street, city, postalCode, country, phone, state, zipCode)
            );
        }

        public void Update(string name, Guid? vendorId, string street, string city, string state, string zipCode, string country, string phone, string postalCode)
        {
            Name = new Name(name);
            VendorId = vendorId;
            Address = new Address(street, city, postalCode, country, phone, state, zipCode);
        }

        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(450)]
        public Name Name { get; set; }

        public Guid? VendorId { get; set; }

        public Address Address { get; set; }
    }
}
