using EShop.Module.Core.Contract.Dtos;
using System.Collections.Generic;

namespace Eshop.Module.Shipping.Services
{
    public class GetShippingPriceRequest
    {
        public ShippingAddressDto ShippingAddress { get; set; }

        public ShippingAddressDto WarehouseAddress { get; set; }

        public IList<ShippingItem> ShippingItem { get; set; } = new List<ShippingItem>();

        public decimal OrderAmount { get; set; }
    }
}
