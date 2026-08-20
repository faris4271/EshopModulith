using Eshop.Module.Localization.Data;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Localization;

namespace Eshop.Module.Localization
{
    public class EfStringLocalizerFactory : IStringLocalizerFactory
    {
        private HybridCache _cacheService;
        private readonly IServiceProvider _serviceProvider;
        private readonly LocalizationDbContext _DbContext;

        public EfStringLocalizerFactory(IServiceProvider serviceProvider, HybridCache cacheService, LocalizationDbContext dbContext)
        {
            _serviceProvider = serviceProvider;
            _cacheService = cacheService;
            _DbContext = dbContext;
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            return new EfStringLocalizer(_cacheService, _serviceProvider, _DbContext);
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            return new EfStringLocalizer(_cacheService, _serviceProvider, _DbContext);
        }
    }
}