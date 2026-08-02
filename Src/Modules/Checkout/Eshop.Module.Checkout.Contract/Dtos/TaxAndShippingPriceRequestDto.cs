using EShop.Module.Core.Contract.Dtos;
using System;

namespace Eshop.Module.Checkout.Contract.Dtos
{
    public record TaxAndShippingPriceRequestDto
    {
        public string SelectedShippingMethodName { get; set; }

        public ShippingAddressDto NewShippingAddress { get; set; }

        public ShippingAddressDto NewBillingAddress { get; set; }

        public Guid ExistingShippingAddressId { get; set; }
    }
}
