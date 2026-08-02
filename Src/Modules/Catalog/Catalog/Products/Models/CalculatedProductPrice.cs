using EShop.Module.Core.Contract.Services;

namespace Catalog.Products.Models
{
    public class CalculatedProductPrice
    {
        private ICurrencyService _currencyService;



        public decimal Price { get; set; }

        public decimal? OldPrice { get; set; }

        public int PercentOfSaving { get; set; }

        public string PriceString { get; set; }

        public string OldPriceString { get; set; }
    }
}
