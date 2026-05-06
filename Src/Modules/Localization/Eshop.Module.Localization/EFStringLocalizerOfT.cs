using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Eshop.Module.Localization.Data;
using Eshop.Module.Localization.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Shared.Caching;


namespace Eshop.Module.Localization
{
    public class EfStringLocalizer<T> : IStringLocalizer<T>
    {
        private readonly IServiceProvider _serviceProvider;
        private ICacheService _cacheService;

        public EfStringLocalizer(IServiceProvider serviceProvider, ICacheService resourcesCache)
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
            var resources =  LoadResources(culture).Result;

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
            var resources = LoadResources(culture).Result;
            var value = resources.SingleOrDefault(r => r.Key == name)?.Value;

            return value;
        }

        private async Task<IList<Resource>> LoadResources(string culture)
        {
            var resources = await _cacheService.GetOrSetAsync(GetCasheKey(culture), async () =>
            {
                using var scope = _serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<LocalizationDbContext>();
                var resources = await dbContext.Resources.Where(r => r.CultureId == culture).ToListAsync();
                return resources;
            });
            return resources;
        }

        private string GetCasheKey(string culture)
        {
            return $"LocalizationResources_{culture}";
        }
    }
}