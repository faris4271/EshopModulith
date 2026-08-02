using Shared.DDD;

namespace Eshop.Module.Core.Models
{
    public class Address : EntityBase<Guid>, IAuditableEntity
    {
        public string ContactName { get; private set; } = string.Empty;
        public string Phone { get; private set; } = string.Empty;
        public string AddressLine1 { get; private set; } = string.Empty;
        public string? AddressLine2 { get; private set; }
        public string CityName { get; private set; } = string.Empty;
        public string ZipCode { get; private set; } = string.Empty;
        public Guid CountryId { get; private set; } = Guid.Empty;
        public Guid StateOrProvinceId { get; private set; } = Guid.Empty;
        public Guid? DistrictId { get; private set; }

        public Country Country { get; private set; } = null!;
        public StateOrProvince StateOrProvince { get; private set; } = null!;
        public District? District { get; private set; }

        public DateTimeOffset CreatedOn { get; set; }
        public string? CreatedById { get; set; }
        public DateTimeOffset? LatestUpdatedOn { get; set; }
        public string? LatestUpdatedById { get; set; }

        private Address() { }

        public static Address Create(
            string contactName,
            string phone,
            string addressLine1,
            string addressLine2,
            string cityName,
            string zipCode,
            Guid countryId,
            Guid stateOrProvinceId,
            Guid? districtId)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(contactName);
            ArgumentException.ThrowIfNullOrWhiteSpace(phone);
            ArgumentException.ThrowIfNullOrWhiteSpace(addressLine1);
            ArgumentException.ThrowIfNullOrWhiteSpace(cityName);
            ArgumentException.ThrowIfNullOrWhiteSpace(zipCode);


            var address = new Address
            {
                Id = Guid.NewGuid(),
                ContactName = contactName,
                Phone = phone,
                AddressLine1 = addressLine1,
                AddressLine2 = addressLine2 ?? string.Empty,
                CityName = cityName,
                ZipCode = zipCode,
                CountryId = countryId,
                StateOrProvinceId = stateOrProvinceId,
                DistrictId = districtId,
                CreatedOn = DateTimeOffset.UtcNow
            };

            return address;
        }

        public void UpdateDetails(
            string contactName,
            string phone,
            string addressLine1,
            string addressLine2,
            string cityName,
            string zipCode,
            Guid countryId,
            Guid stateOrProvinceId,
            Guid? districtId = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(contactName);
            ArgumentException.ThrowIfNullOrWhiteSpace(phone);
            ArgumentException.ThrowIfNullOrWhiteSpace(addressLine1);
            ArgumentException.ThrowIfNullOrWhiteSpace(cityName);
            ArgumentException.ThrowIfNullOrWhiteSpace(zipCode);


            ContactName = contactName;
            Phone = phone;
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2 ?? string.Empty;
            CityName = cityName;
            ZipCode = zipCode;
            CountryId = countryId;
            StateOrProvinceId = stateOrProvinceId;
            DistrictId = districtId;
            LatestUpdatedOn = DateTimeOffset.UtcNow;
        }

        public string GetFullAddress()
        {
            var parts = new List<string> { AddressLine1 };

            if (!string.IsNullOrEmpty(AddressLine2))
                parts.Add(AddressLine2);

            parts.Add(CityName);
            parts.Add(StateOrProvince?.Name);

            if (DistrictId != Guid.Empty)
                parts.Add(District?.Name);

            parts.Add(ZipCode);
            parts.Add(Country?.Name ?? CountryId.ToString());

            return string.Join(", ", parts.Where(p => !string.IsNullOrEmpty(p)));
        }
    }
}