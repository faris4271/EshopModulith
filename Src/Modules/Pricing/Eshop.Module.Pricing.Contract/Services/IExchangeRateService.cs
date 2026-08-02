using System.Threading;
using System.Threading.Tasks;

namespace Eshop.Module.Pricing.Contract.Services
{
    public interface IExchangeRateService
    {
        /// <summary>
        /// Converts an amount from one currency to another using cached rates.
        /// </summary>
        Task<decimal> ConvertPriceAsync(
            decimal amount,
            string fromCurrency,
            string toCurrency,
            CancellationToken ct = default);

        /// <summary>
        /// Background job method to fetch latest rates from external API and sync DB + Cache.
        /// </summary>
        Task SyncLatestRatesAsync(CancellationToken ct = default);
    }
}
