using Eshop.Module.Checkout.Contract.Dtos;
using Eshop.Module.Checkout.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Eshop.Module.Checkout.Contract.Services
{
    public interface ICheckoutService
    {
        Task<CheckoutDto> GetCheckoutDetails(Guid checkoutId);
        Task<decimal> GetTax(Guid checkoutId, Guid countryId, Guid stateOrProvinceId, string zipCode);
        Task<CheckoutTaxAndShippingPriceDto> UpdateTaxAndShippingPrices(Guid checkoutId, TaxAndShippingPriceRequestDto model);
        Task<CreatCheckoutResultDto> Create(Guid customerId, Guid createdById, IList<CartItemToCheckoutDto> cartItems, string couponCode);
    }
}
