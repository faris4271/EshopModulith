namespace Eshop.Module.Pricing.Services;

using Eshop.Module.Pricing;
using Eshop.Module.Pricing.Contract.Services;
using Eshop.Module.Pricing.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Caching;
using System.Net.Http.Json;

public class ExchangeRateService : IExchangeRateService
{
    private readonly PricingDbContext _dbContext;
    private readonly HttpClient _httpClient;
    private readonly ICacheService _cache;
    private readonly ILogger<ExchangeRateService> _logger;
    private readonly ExchangeRateApiOptions _options;

    private const string CacheKeyPrefix = "exchange_rate_";

    public ExchangeRateService(
        PricingDbContext dbContext,
        HttpClient httpClient,
        ICacheService cache,
        ILogger<ExchangeRateService> logger,
        IOptions<ExchangeRateApiOptions> options)
    {
        _dbContext = dbContext;
        _httpClient = httpClient;
        _cache = cache;
        _logger = logger;
        _options = options.Value;
    }


    public async Task<decimal> ConvertPriceAsync(
        decimal amount,
        string fromCurrency,
        string toCurrency,
        CancellationToken ct = default)
    {
        var from = fromCurrency.ToUpperInvariant().Trim();
        var to = toCurrency.ToUpperInvariant().Trim();

        // 1. Same currency, no conversion needed
        if (from == to || amount == 0)
            return amount;

        // 2. Try fetching direct conversion rate
        var directRate = await GetRateAsync(from, to, ct);
        if (directRate.HasValue)
        {
            return Math.Round(amount * directRate.Value, 2, MidpointRounding.AwayFromZero);
        }

        // 3. Fallback: Cross-Rate Calculation via USD (from -> USD -> to)
        var rateToUsd = await GetRateAsync(from, "USD", ct);
        var rateUsdToTarget = await GetRateAsync("USD", to, ct);

        if (rateToUsd.HasValue && rateUsdToTarget.HasValue)
        {
            var combinedRate = rateToUsd.Value * rateUsdToTarget.Value;
            return Math.Round(amount * combinedRate, 2, MidpointRounding.AwayFromZero);
        }

        _logger.LogError("Exchange rate missing for pair {From} -> {To}", from, to);
        throw new InvalidOperationException($"Exchange rate not found for currency conversion: {from} to {to}");
    }

    /// <summary>
    /// Fetches live rates from the external API and saves them to DB + Cache.
    /// Call this from a Background Worker or Quartz/Hangfire job.
    /// </summary>
    public async Task SyncLatestRatesAsync(CancellationToken ct = default)
    {
        try
        {
            _logger.LogInformation("Starting Exchange Rates Synchronization...");

            var baseCurrency = _options.DefaultBaseCurrency.ToUpperInvariant();
            var url = $"{_options.BaseUrl}{_options.ApiKey}/latest/{baseCurrency}";

            var response = await _httpClient.GetFromJsonAsync<ExchangeRateApiResponse>(url, ct);

            if (response == null || response.result != "success")
            {
                _logger.LogError("Failed to fetch exchange rates from provider API.");
                return;
            }

            var existingRates = await _dbContext.ExchangeRates.ToListAsync(ct);

            foreach (var (targetCurrency, rateValue) in response.conversion_rates)
            {
                var target = targetCurrency.ToUpperInvariant();
                var existing = existingRates.FirstOrDefault(r =>
                    r.FromCurrency == baseCurrency && r.ToCurrency == target);

                if (existing == null)
                {
                    var newRate = ExchangeRate.Create(baseCurrency, target, rateValue);
                    await _dbContext.ExchangeRates.AddAsync(newRate, ct);
                }
                else
                {
                    existing.UpdateRate(rateValue);
                }

                // Update Memory Cache instantly
                UpdateCache(baseCurrency, target, rateValue);
            }

            await _dbContext.SaveChangesAsync(ct);
            _logger.LogInformation("Exchange Rates successfully synced and cached!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while syncing exchange rates from external API.");
        }
    }

    #region Helper Methods

    private async Task<decimal?> GetRateAsync(string from, string to, CancellationToken ct)
    {
        var cacheKey = $"{CacheKeyPrefix}{from}_{to}";

        var rateFromCache = await _cache.GetOrSetAsync(cacheKey, async () =>
        {
            var rateFromDb = await _dbContext.ExchangeRates
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.FromCurrency == from && r.ToCurrency == to, ct);
            return rateFromDb?.Rate;
        }, TimeSpan.FromHours(12));




        return rateFromCache;
    }

    private void UpdateCache(string from, string to, decimal rate)
    {
        var cacheKey = $"{CacheKeyPrefix}{from}_{to}";
        _cache.SetItem(cacheKey, rate, TimeSpan.FromHours(12));
    }

    #endregion
}