using CatalogContract.Dtos;
using EShop.Module.Core.Contract.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eshop.Module.Basket.Contract.Dtos
{
    public class CartItemDto
    {
        private readonly ICurrencyService _currencyService;

        public CartItemDto(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductImage { get; set; }

        public CalculatedProductPrice CalculatedProductPrice { get; set; }

        public decimal ProductPrice { get; set; }

        public string ProductPriceString => _currencyService.FormatCurrency(ProductPrice);

        public int ProductStockQuantity { get; set; }

        public bool ProductStockTrackingIsEnabled { get; set; }

        public bool IsProductAvailabeToOrder { get; set; }

        public int Quantity { get; set; }

        public decimal Total => Quantity * ProductPrice;

        public string TotalString => _currencyService.FormatCurrency(Total);

        public IEnumerable<ProductVariationOptionDto> VariationOptions { get; set; } = new List<ProductVariationOptionDto>();

   
    }
}
