using Eshop.Module.Shipping.Contract.Dtos;
using Eshop.Module.Shipping.Data;
using Eshop.Module.Shipping.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Abstraction;

namespace Eshop.Module.Shipping.Services
{
    public class ShippingPriceService : IShippingPriceService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IGenericeRepository<ShippingProvider, ShippingDbContext> _shippingProviderRepository;

        public ShippingPriceService(IServiceProvider serviceProvider, IGenericeRepository<ShippingProvider, ShippingDbContext> shippingProviderRepository)
        {
            _serviceProvider = serviceProvider;
            _shippingProviderRepository = shippingProviderRepository;
        }

        public async Task<IList<ShippingPriceDto>> GetApplicableShippingPrices(GetShippingPriceRequest request)
        {
            var applicableShippingPrices = new List<ShippingPriceDto>();

            var query = await _shippingProviderRepository.Query();
            var providers = await query.ToListAsync();
            var shippingRateServices = _serviceProvider.GetServices<IShippingPriceServiceProvider>();

            foreach (var provider in providers)
            {
                if (!provider.IsEnabled)
                {
                    continue;
                }

                if (!provider.ToAllShippingEnabledCountries)
                {
                    if (!provider.OnlyCountryIds.Contains(request.ShippingAddress.CountryId.ToString()))
                    {
                        continue;
                    }
                }

                if (!provider.ToAllShippingEnabledStatesOrProvinces)
                {
                    if (!provider.OnlyStateOrProvinceIds.Contains(request.ShippingAddress.StateOrProvinceId))
                    {
                        continue;
                    }
                }

                var priceServiceType = Type.GetType(provider.ShippingPriceServiceTypeName);
                var priceService = shippingRateServices.Where(x => x.GetType() == priceServiceType).FirstOrDefault();
                var response = await priceService.GetShippingPrices(request, provider);
                applicableShippingPrices.AddRange(response.ApplicablePrices);

            }

            return applicableShippingPrices;
        }
    }
}
