using Eshop.Module.Shipping.Contract.Dtos;

namespace Eshop.Module.Shipping.Services
{
    public interface IShippingPriceService
    {
        Task<IList<ShippingPriceDto>> GetApplicableShippingPrices(GetShippingPriceRequest request);
    }
}
