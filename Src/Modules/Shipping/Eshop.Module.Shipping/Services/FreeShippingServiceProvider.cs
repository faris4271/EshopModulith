using Eshop.Module.Pricing.Contract.Services;
using Eshop.Module.Shipping.Contract.Dtos;
using Eshop.Module.Shipping.Models;
using Newtonsoft.Json;

namespace Eshop.Module.Shipping.Services
{
    public class FreeShippingServiceProvider : IShippingPriceServiceProvider
    {
        private readonly ICurrencyService _currencyService;

        public FreeShippingServiceProvider(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        public Task<GetShippingPriceResponse> GetShippingPrices(GetShippingPriceRequest request, ShippingProvider provider)
        {
            var response = new GetShippingPriceResponse { IsSuccess = true };

            var freeShippingSetting = JsonConvert.DeserializeObject<FreeShippingSetting>(provider.AdditionalSettings);

            if (request.OrderAmount < freeShippingSetting.MinimumOrderAmount)
            {
                return Task.FromResult(response);
            }

            response.ApplicablePrices.Add(new ShippingPriceDto
            {
                Name = "Free",
                Price = 0,

                Description = "Free shipping"

            });

            return Task.FromResult(response);
        }
    }
}
