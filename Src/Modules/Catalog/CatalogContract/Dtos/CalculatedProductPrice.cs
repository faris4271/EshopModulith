using EShop.Module.Core.Contract.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogContract.Dtos
{
    public class CalculatedProductPrice
    {
        private ICurrencyService _currencyService;

        public CalculatedProductPrice(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        public decimal Price { get; set; }

        public decimal? OldPrice { get; set; }

        public int PercentOfSaving { get; set; }

        public string PriceString => _currencyService.FormatCurrency(Price);

        public string OldPriceString => OldPrice.HasValue ? _currencyService.FormatCurrency(OldPrice.Value) : string.Empty;
    }
}
