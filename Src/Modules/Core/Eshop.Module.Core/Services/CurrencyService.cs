using EShop.Module.Core.Contract.Services;
using Microsoft.Extensions.Configuration;
using System.Globalization;

namespace Eshop.Module.Core.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly IConfiguration _config;

        public CurrencyService(IConfiguration config)
        {
            _config = config;

            // Read culture string from configuration using GetSection(...).Value
            // Use colon ":" as typical separator for nested keys in IConfiguration
            var currencyCultureString = _config.GetSection("Global:CurrencyCulture")?.Value;

            if (string.IsNullOrWhiteSpace(currencyCultureString))
            {
                // Fallback to current culture when configuration is missing or empty
                CurrencyCulture = CultureInfo.CurrentCulture;
            }
            else
            {
                // Try to create CultureInfo, fallback to CurrentCulture on failure
                try
                {
                    CurrencyCulture = new CultureInfo(currencyCultureString);
                }
                catch (CultureNotFoundException)
                {
                    CurrencyCulture = CultureInfo.CurrentCulture;
                }
            }
        }

        public CultureInfo CurrencyCulture { get; }

        public string FormatCurrency(decimal value)
        {
            // Read decimal places from configuration using GetSection(...).Value
            var decimalPlaceString = _config.GetSection("Global:CurrencyDecimalPlace")?.Value;

            // Default decimal places to 2
            int decimalPlace = 2;
            if (!string.IsNullOrWhiteSpace(decimalPlaceString))
            {
                if (!int.TryParse(decimalPlaceString, out decimalPlace))
                {
                    decimalPlace = 2;
                }
            }

            return value.ToString($"C{decimalPlace}", CurrencyCulture);
        }
    }
}
