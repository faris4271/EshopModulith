using Eshop.Module.Localization.Data;
using Eshop.Module.Localization.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace Eshop.Module.Localization
{
    internal class EfStringLocalizer : IStringLocalizer
    {
        private static readonly HybridCacheEntryOptions StringLocalizer = new()
        {
            Expiration = TimeSpan.FromHours(1),
            LocalCacheExpiration = TimeSpan.FromMinutes(2),
        };
        private readonly HybridCache _cacheService;

        private readonly IServiceProvider _serviceProvider;

        private readonly LocalizationDbContext _dbContext;

        public EfStringLocalizer(HybridCache cacheService, IServiceProvider serviceProvider, LocalizationDbContext dbContext)
        {
            _cacheService = cacheService;
            _serviceProvider = serviceProvider;
            _dbContext = dbContext;
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

                    var freshResourcesCache = await LoadResources(culture, CancellationToken.None);
                    freshResourcesCache.Add(res);
                    await _cacheService.SetAsync(GetCasheKey(culture), freshResourcesCache);
                }
            }
        }

        private async Task<string> GetString(string name)
        {
            var culture = CultureInfo.CurrentUICulture.Name;
            var resources = await LoadResources(culture, CancellationToken.None);
            var value = resources.SingleOrDefault(r => r.Key == name)?.Value;

            if (value == null)
            {
                AutoRegisterNewString(name, culture);
            }

            return value;
        }

        private async Task<IList<Resource>> LoadResources(string culture, CancellationToken ct)
        {
            var state = new LocalizationFactoryState(_dbContext, culture);
            var tags = new[] { "Culture", "CultureId" };
            var resources = await _cacheService.GetOrCreateAsync(
                GetCasheKey(culture),
                state,
                LoadResourcesFromDb,
                StringLocalizer,
                tags,
                ct
            ).AsTask();

            return resources;
        }

        private string GetCasheKey(string culture)
        {
            return $"LocalizationResources_{culture}";
        }
        private async ValueTask<List<Resource>> LoadResourcesFromDb(LocalizationFactoryState state, CancellationToken ct)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var resourceRepository = scope.ServiceProvider.GetRequiredService<LocalizationDbContext>();
                return await resourceRepository.Resources.Where(x => x.CultureId == state.CultureId).ToListAsync();
            }

        }
        private readonly record struct LocalizationFactoryState(LocalizationDbContext DbContext, string CultureId);

    }
}
