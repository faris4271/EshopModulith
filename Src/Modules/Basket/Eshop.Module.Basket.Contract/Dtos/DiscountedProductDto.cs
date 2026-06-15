using System;
using System.Collections.Generic;
using System.Text;

namespace Eshop.Module.Basket.Contract.Dtos
{
    public class DiscountedProductDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public decimal DiscountAmount { get; set; }
    }
}
