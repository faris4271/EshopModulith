using Eshop.Module.Shipping.Contract.Dtos;
using System.Collections.Generic;

namespace Eshop.Module.Checkout.Contract.Dtos
{
    public record CheckoutTaxAndShippingPriceDto
    {
        public bool IsProductPriceIncludedTax { get; set; }

        public string SelectedShippingMethodName { get; set; }
        public IList<ShippingPriceDto> ShippingPrices { get; set; }

        public CheckoutDto CheckoutDto { get; set; }
    }

}
