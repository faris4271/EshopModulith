using Eshop.Module.Shipping.Models;
using System.Threading.Tasks;

namespace Eshop.Module.Shipping.Services
{
    public interface IShippingPriceServiceProvider
    {
        Task<GetShippingPriceResponse> GetShippingPrices(GetShippingPriceRequest request, ShippingProvider provider);
    }
}
