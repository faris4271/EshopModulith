using EShop.Module.Core.Contract.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eshop.Module.Basket.Contract.Dtos
{
    public class CartDto
    {

        private readonly ICurrencyService _currencyService;

        public CartDto(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        public string CouponCode { get; set; }

        public decimal SubTotal { get; set; }

        public string SubTotalString { get { return _currencyService.FormatCurrency(SubTotal); } }

        public decimal Discount { get; set; }

        public string DiscountString { get { return _currencyService.FormatCurrency(Discount); } }

        public string CouponValidationErrorMessage { get; set; }

        public decimal SubTotalWithDiscount
        {
            get
            {
                return SubTotal - Discount;
            }
        }

        public string SubTotalWithDiscountString { get { return _currencyService.FormatCurrency(SubTotalWithDiscount); } }

        public IList<CartItemDto> Items { get; set; } = new List<CartItemDto>();

        public bool IsValid
        {
            get
            {
                foreach (var item in Items)
                {
                    if (!item.IsProductAvailabeToOrder)
                    {
                        return false;
                    }

                    if (item.ProductStockTrackingIsEnabled && item.ProductStockQuantity < item.Quantity)
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }
}
