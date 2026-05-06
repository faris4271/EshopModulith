using Eshop.Module.Localization.Data;
using Eshop.Module.Localization.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Shared.Caching;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Eshop.Module.Localization
{
    internal class EfStringLocalizer : IStringLocalizer
    {
        private readonly ICacheService _cacheService;

        private readonly IServiceProvider _serviceProvider;

        public EfStringLocalizer(ICacheService cacheService, IServiceProvider serviceProvider)
        {
            _cacheService = cacheService;
            _serviceProvider = serviceProvider;
        }

        public LocalizedString this[string name]
        {
            get
            {
                var value = GetString(name).ConfigureAwait(false).GetAwaiter().GetResult();
                return new LocalizedString(name, value ?? name, value == null);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var format = GetString(name).ConfigureAwait(false).GetAwaiter().GetResult();
                var value = string.Format(format ?? name, arguments);
                return new LocalizedString(name, value, format == null);
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            throw new NotImplementedException();
        }

        private async Task AutoRegisterNewString(string name, string culture)
        {
            if (culture != "en-US") //GlobalConfiguration.DefaultCulture
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var resourceRepository = scope.ServiceProvider.GetRequiredService<LocalizationDbContext>();
                    //Avoid duplications
                    if (resourceRepository.Resources.Any(x => x.CultureId == culture && x.Key.ToLower() == name.ToLower()))
                    {
                        return;
                    }
                    var res = new Resource()
                    {
                        CultureId = culture,
                        Key = name,
                        Value = name
                    };

                    resourceRepository.Add(res);
                    resourceRepository.SaveChanges();

                    var freshResourcesCache =await LoadResources(culture);
                    freshResourcesCache.Add(res);
                    await _cacheService.SetItemAsync(GetCasheKey(culture), freshResourcesCache);
                }
            }
        }

        private async Task<string> GetString(string name)
        {
            var culture = CultureInfo.CurrentUICulture.Name;
            var resources = await LoadResources(culture);
            var value = resources.SingleOrDefault(r => r.Key == name)?.Value;

            if (value == null)
            {
                AutoRegisterNewString(name, culture);
            }

            return value;
        }

        private async Task<IList<Resource>> LoadResources(string culture)
        {
            var resources =await _cacheService.GetOrSetAsync(GetCasheKey(culture), async () =>
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
