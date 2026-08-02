using Eshop.Module.Core.Data;
using Eshop.Module.Core.Models;
using EShop.Module.Core.Contract.Dtos;
using EShop.Module.Core.Contract.Feature.GetAddress;
using Microsoft.EntityFrameworkCore;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Eshop.Module.Core.Feature.Addresses.GetAddress
{
    internal class GetAddressByIdsHandler(IGenericeRepository<Address, CoreDbContext> _repository) : IQueryHandler<GetAddressByIdsQuery, List<ShippingAddressDto>>
    {
        public async Task<Result<List<ShippingAddressDto>>> Handle(GetAddressByIdsQuery request, CancellationToken cancellationToken)
        {
            var query = await _repository.Query();
            var addresses = query.Include(x => x.District)
                         .Include(x => x.StateOrProvince)
                         .Include(x => x.Country)
                         .Where(x => request.AddressIds.Contains(x.Id));

            var addressDtos = addresses.Select(x => new ShippingAddressDto
            {
                AddressLine1 = x.AddressLine1,
                CityName = x.CityName,
                ContactName = x.ContactName,
                CountryId = x.CountryId,
                StateOrProvinceId = x.StateOrProvinceId,
                DistrictId = x.DistrictId,
                Phone = x.Phone,
                ZipCode = x.ZipCode,
                CountryName = x.Country.Name,
                StateOrProvinceName = x.StateOrProvince.Name,
                DistrictName = x.District != null ? x.District.Name : null,
                IsCityEnabled = x.Country.IsCityEnabled,
                IsDistrictEnabled = x.Country.IsDistrictEnabled,
                IsZipCodeEnabled = x.Country.IsZipCodeEnabled,


            }).ToList();

            return Result.Success(addressDtos);
        }


    }
}
