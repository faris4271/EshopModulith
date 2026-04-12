using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Shared.Caching
{
    internal class HybridCacheService : ICacheService
    {
        private static readonly Encoding Utf8 = Encoding.UTF8;
        private static readonly JsonSerializerOptions JsonOpts = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        private readonly IMemoryCache _memoryCache;
        private readonly IDistributedCache _distributedCache;
        private readonly ILogger<HybridCacheService> _logger;
        private readonly CachingOptions _opts;
        public T? GetItem<T>(string key)
            => GetItemAsync<T>(key).GetAwaiter().GetResult();


        public async Task<T?> GetItemAsync<T>(string key, CancellationToken ct = default)
        {
           key = Normalize(key);
            try
            {
                if(_memoryCache.TryGetValue<T>(key, out var itemInMemory))
                    return itemInMemory;


                var bytes=await _distributedCache.GetAsync(key, ct);

              var  value=JsonSerializer.Deserialize<T>(Utf8.GetString(bytes), JsonOpts);

                if (value is not null)
                {
                    var expirationOptions = GetMemoryCacheExpiration();
                    var inMemoryTime=
                    _memoryCache.Set(key, value, expirationOptions);
                    _logger.LogInformation("Cached item with key '{Key}'" +
                        " in memory cache for {Expiration} seconds", key, expirationOptions.SlidingExpiration?.TotalSeconds);
                }
                return value;
            }
            catch (Exception ex)
            {
               _logger.LogError(ex, "Error retrieving item with key '{Key}' from cache", key);

                return default;
            }
        }

        public void RefreshItem(string key)
         =>RefreshItemAsync(key).GetAwaiter().GetResult();

        public async Task RefreshItemAsync(string key, CancellationToken ct = default)
        {
            key= Normalize(key);

            try
            {
               await _distributedCache.RefreshAsync(key, ct);
                _logger.LogDebug("Refreshed {Key} in distributed cache", key);
            }
            catch (Exception)
            {

               _logger.LogWarning("Failed to refresh {Key} in distributed cache", key);
            }
        }

        public void RemoveItem(string key)
       => RemoveItemAsync(key).GetAwaiter().GetResult();

        public async Task RemoveItemAsync(string key, CancellationToken ct = default)
        {
            key = Normalize(key);
            try
            {
                // Remove from both caches
                _memoryCache.Remove(key);
                await _distributedCache.RemoveAsync(key, ct).ConfigureAwait(false);
                _logger.LogDebug("Removed {Key} from both caches", key);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogWarning(ex, "Cache remove failed for {Key}", key);
            }
        }

        public void SetItem<T>(string key, T value, TimeSpan? sliding = null)
          => SetItemAsync(key, value, sliding).GetAwaiter().GetResult();

        public async Task SetItemAsync<T>(string key, T value, TimeSpan? sliding = null, CancellationToken ct = default)
        {
            key = Normalize(key);

            try
            {
                var bytes = Utf8.GetBytes(JsonSerializer.Serialize(value, JsonOpts));

                _distributedCache.SetAsync(key, bytes, BuildDistributedEntryOptions(sliding), ct).ConfigureAwait(false);
               

                _memoryCache.Set(key, value, GetMemoryCacheExpiration());

                _logger.LogDebug("Cached {Key} in both memory and distributed caches", key);
            }
            catch (Exception ex) when (ex is not OperationCanceledException) 
            {

                _logger.LogWarning(ex, "Cache set failed for {Key}", key);
            }
        }

        private DistributedCacheEntryOptions BuildDistributedEntryOptions(TimeSpan? sliding)
        {
            var o=new DistributedCacheEntryOptions();

            if(sliding.HasValue)
                o.SetSlidingExpiration(sliding.Value);
            else if(_opts.DefaultSlidingExpiration.HasValue)
                o.SetSlidingExpiration(_opts.DefaultSlidingExpiration.Value);

            if(_opts.DefaultAbsoluteExpiration.HasValue)
                o.SetAbsoluteExpiration(_opts.DefaultAbsoluteExpiration.Value);

            return o;
        }

        private string Normalize(string key)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(key, nameof(key));

            var prefix=_opts.KeyPrefix ?? string.Empty;

            if(prefix.Length == 0)
                return key;

            return key.StartsWith(prefix,StringComparison.Ordinal) ? key : prefix + key;
        }
        private MemoryCacheEntryOptions GetMemoryCacheExpiration()
        {
            var options = new MemoryCacheEntryOptions();

            // Use shorter expiration for memory cache (faster refresh from distributed cache)
            var slidingExpiration = _opts.DefaultSlidingExpiration ?? TimeSpan.FromMinutes(1);
            options.SetSlidingExpiration(TimeSpan.FromSeconds(slidingExpiration.TotalSeconds * 0.8)); // 80% of distributed cache expiration

            return options;
        }
    }
}
