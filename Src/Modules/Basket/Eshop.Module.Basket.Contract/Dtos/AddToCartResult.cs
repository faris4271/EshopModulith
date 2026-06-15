using System;
using System.Collections.Generic;
using System.Text;

namespace Eshop.Module.Basket.Contract.Dtos
{
    public class AddToCartResult
    {
        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public bool Success { get; set; }
    }
}
