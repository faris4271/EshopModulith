using System.Collections.Generic;

namespace Eshop.Module.Pricing.Contract.Dtos
{
    public class CartInfoForCoupon
    {
        public IList<CartItemForCoupon> Items { get; set; } = new List<CartItemForCoupon>();
    }
}