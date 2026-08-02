namespace EShop.Module.Core.Contract.Dtos
{
    public record AddressDto(
        Guid Id,
        string ContactName,
        string Phone,
        string AddressLine1,
        string AddressLine2,
        string CityName,
        string ZipCode,
        string CountryId,
        string StateOrProvinceId,
        string DistrictId
    );
}