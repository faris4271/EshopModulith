using Eshop.Module.Shipping.Contract.Dtos;

namespace Eshop.Module.Shipping.Services
{
    public class GetShippingPriceResponse
    {
        public List<ShippingPriceDto> ApplicablePrices = new List<ShippingPriceDto>();

        public string CarrierName { get; set; }

        public bool IsSuccess { get; set; }

        public string Error { get; set; }
    }
}
