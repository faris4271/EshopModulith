using Eshop.Module.Localization.Data;
using Eshop.Module.Localization.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Shared.Caching;
using System.Globalization;

namespace Eshop.Module.Localization
{
    public class EfStringLocalizer<T> : IStringLocalizer<T>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly HybridCache _cacheService;

        private static readonly HybridCacheEntryOptions LocalizationEntryOptions = new()
        {
            Expiration = TimeSpan.FromHours(1),
            LocalCacheExpiration = TimeSpan.FromMinutes(2),
        };

        public EfStringLocalizer(IServiceProvider serviceProvider, HybridCache resourcesCache)
        {
            _serviceProvider = serviceProvider;
            _cacheService = resourcesCache;
        }

        public LocalizedString this[string name]
        {
            get
            {
                var value = GetString(name);
                return new LocalizedString(name, value ?? name, value == null);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var format = GetString(name);
                var value = string.Format(format ?? name, arguments);
                return new LocalizedString(name, value, format == null);
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            var culture = CultureInfo.CurrentUICulture.Name;
            var resources = LoadResources(culture).GetAwaiter().GetResult();

            return resources.Select(r => new LocalizedString(r.Key, r.Value, true));
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            CultureInfo.DefaultThreadCurrentCulture = culture;
            return new EfStringLocalizer<T>(_serviceProvider, _cacheService);
        }

        private string GetString(string name)
        {
            var culture = CultureInfo.CurrentUICulture.Name;
            var resources = LoadResources(culture).GetAwaiter().GetResult();
            var value = resources.FirstOrDefault(r => r.Key == name)?.Value;

            return value;
        }

        private async Task<IList<Resource>> LoadResources(string culture, CancellationToken ct = default)
        {
            var tags = new[] { CacheKeys.Tags.Localization, CacheKeys.Tags.LocalizationEntry(culture) };

            var resources = await _cacheService.GetOrCreateAsync(
                GetCacheKey(culture),
                culture,
                LoadResourceFromDb,
                LocalizationEntryOptions,
                tags,
                ct);

            return resources;
        }

        private async ValueTask<IList<Resource>> LoadResourceFromDb(string culture, CancellationToken token)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<LocalizationDbContext>();

            var resources = await dbContext.Resources
                .Where(r => r.CultureId == culture)
                .ToListAsync(token);

            return resources;
        }

        private string GetCacheKey(string culture)
        {
            return $"LocalizationResources_{culture}";
        }
    }
}