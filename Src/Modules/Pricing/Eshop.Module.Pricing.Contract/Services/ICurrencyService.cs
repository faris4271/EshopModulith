namespace Eshop.Module.Pricing.Contract.Services;

using Shared.DDD;
using System.Globalization;

public interface ICurrencyService
{

    string FormatCurrency(Money money, CultureInfo? targetCulture = null);

    int GetDecimalPlaces(string currencyCode);
}