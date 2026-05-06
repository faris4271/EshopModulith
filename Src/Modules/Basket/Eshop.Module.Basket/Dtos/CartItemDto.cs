using CatalogContract.Dtos;
using EShop.Module.Core.Contract.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eshop.Module.Basket.Dtos
{
    internal class CartItemDto
    {
        private readonly ICurrencyService _currencyService;

        public CartItemDto(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        public long Id { get; set; }

        public long ProductId { get; set; }

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

        public static IEnumerable<ProductVariationOptionDto> GetVariationOption(ProductVariationDto variation)
        {
            if (variation == null)
            {
                return new List<ProductVariationOptionDto>();
            }

            return variation.OptionCombinations.Select(x => new ProductVariationOptionDto
            {
                OptionName = x.OptionName,
                OptionValue = x.Value
            });
        }
    }
}
