using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
namespace Shared.Caching
{
    internal class DistributedCacheService : ICacheService
    {
        private readonly CachingOptions _cachingOptions;

        private readonly IDistributedCache _Cache;

        private readonly Encoding utf8;

        private readonly ILogger<DistributedCacheService> _logger;
        private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        public DistributedCacheService(CachingOptions cachingOptions,
            IDistributedCache cache, Encoding utf8,
            ILogger<DistributedCacheService> logger)
        {
            _cachingOptions = cachingOptions;
            _Cache = cache;
            this.utf8 = utf8;
            _logger = logger;
        }

        public async Task<T?> GetItemAsync<T>(string key, CancellationToken ct = default)
        {
            var normalizedKey = Normalize(key);
            try
            {


                var data = await _Cache.GetAsync(normalizedKey).ConfigureAwait(false);

                if (data == null)
                    return default;

                return System.Text.Json.JsonSerializer.Deserialize<T>(utf8.GetString(data), _jsonOptions);

            }
            catch (Exception ex)
            {

                _logger.LogWarning(ex, "Cache get failed for {Key}", key);
                return default;
            }
        }

        public void RefreshItem(string key)
        => RefreshItemAsync(key).GetAwaiter().GetResult();

        public async Task RefreshItemAsync(string key, CancellationToken ct = default)
        {
            key = Normalize(key);
            try
            {
                await _Cache.RefreshAsync(key, ct).ConfigureAwait(false);
                _logger.LogDebug("Refreshed {Key}", key);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            { _logger.LogWarning(ex, "Cache refresh failed for {Key}", key); }
        }

        public void RemoveItem(string key)
       => RemoveItemAsync(key).GetAwaiter().GetResult();

        public async Task RemoveItemAsync(string key, CancellationToken ct = default)
        {
            key = Normalize(key);
            try { await _Cache.RemoveAsync(key, ct).ConfigureAwait(false); }
            catch (Exception ex) when (ex is not OperationCanceledException)
            { _logger.LogWarning(ex, "Cache remove failed for {Key}", key); }
        }

        public void SetItem<T>(string key, T value, TimeSpan? sliding = null)
       => SetItemAsync(key, value, sliding).GetAwaiter().GetResult();

        public async Task SetItemAsync<T>(string key, T value, TimeSpan? sliding = null, CancellationToken ct = default)
        {
            key = Normalize(key);

            try
            {
                var data = utf8.GetBytes(JsonSerializer.Serialize(value, _jsonOptions));

                await _Cache.SetAsync(key, data, BuildEntryOptions(sliding), ct);
                _logger.LogInformation("Cache item set for key: {Key}", key);




            }
            catch (Exception ex)
            {

                _logger.LogWarning(ex, "Cache set failed for {Key}", key);
            }

        }

        private string Normalize(string key)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(key, nameof(key));

            var prefix = _cachingOptions.KeyPrefix ?? string.Empty;

            if (prefix.Length == 0)
                return key;

            return key.StartsWith(prefix, StringComparison.Ordinal) ? key : prefix + key;


        }

        public T? GetItem<T>(string key)
        => GetItemAsync<T>(key).GetAwaiter().GetResult();

        private DistributedCacheEntryOptions BuildEntryOptions(TimeSpan? sliding)
        {
            var op = new DistributedCacheEntryOptions();

            if (sliding.HasValue)
                op.SetSlidingExpiration(sliding.Value);

            else if (_cachingOptions.DefaultSlidingExpiration.HasValue)
                op.SetSlidingExpiration(_cachingOptions.DefaultSlidingExpiration.Value);

            if (_cachingOptions.DefaultAbsoluteExpiration.HasValue)
                op.SetAbsoluteExpiration(_cachingOptions.DefaultAbsoluteExpiration.Value);

            return op;

        }
    }
}
