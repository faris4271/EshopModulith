namespace Shared.Identity
{
    public record Address
    {


        public Address(string street, string city, string postalCode,
            string country, string phone, string state, string zipCode)
        {
            Street = street;
            City = city;
            PostalCode = postalCode;
            Country = country;
            Phone = phone;
            State = state;
            ZipCode = zipCode;
        }

        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Phone { get; set; }

        public string State { get; set; } = string.Empty;
        public string ZipCode { get; set; }

    }
}
