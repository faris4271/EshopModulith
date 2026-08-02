using System;

namespace Eshop.Module.Pricing.Contract.Dtos
{
    public class CartItemForCoupon
    {
        public Guid ProductId { get; set; }

        public int Quantity { get; set; }
    }
}