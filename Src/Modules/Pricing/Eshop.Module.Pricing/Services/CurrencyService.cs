namespace Eshop.Module.Pricing.Services;

using Eshop.Module.Pricing.Contract.Services;
using Microsoft.Extensions.Configuration;
using Shared.DDD;
using System.Globalization;

public class CurrencyService : ICurrencyService
{
    private readonly IConfiguration _config;

    public CurrencyService(IConfiguration config)
    {
        _config = config;
    }

    public string FormatCurrency(Money money, CultureInfo? targetCulture = null)
    {
        ArgumentNullException.ThrowIfNull(money);

        var culture = targetCulture ?? CultureInfo.CurrentUICulture;

        var numberFormat = (NumberFormatInfo)culture.NumberFormat.Clone();

        numberFormat.CurrencySymbol = ResolveCurrencySymbol(money.Currency, culture);

        int decimalPlaces = GetDecimalPlaces(money.Currency);

        return money.Amount.ToString($"C{decimalPlaces}", numberFormat);

    }

    public int GetDecimalPlaces(string currencyCode)
    {
        if (string.IsNullOrWhiteSpace(currencyCode))
            return _config.GetValue<int>("Global.DefaultCurrencyDecimalPlace", 2);

        return currencyCode.ToUpperInvariant() switch
        {
            "KWD" or "BHD" or "OMR" or "TND" or "JOD" => 3,
            "JPY" or "KRW" or "VND" or "CLP" or "PYG" => 0,
            _ => _config.GetValue<int>("Global.DefaultCurrencyDecimalPlace", 2)
        };
    }

    private static string ResolveCurrencySymbol(string currencyCode, CultureInfo culture)
    {

        try
        {
            var region = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                .Select(c => new RegionInfo(c.Name))
                .FirstOrDefault(r => r.ISOCurrencySymbol.Equals(currencyCode, StringComparison.OrdinalIgnoreCase));

            return region?.CurrencySymbol ?? currencyCode;
        }
        catch
        {
            return currencyCode;
        }
    }
}