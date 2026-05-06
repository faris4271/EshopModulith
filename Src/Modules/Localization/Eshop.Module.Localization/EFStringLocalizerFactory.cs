using System;
using Eshop.Module.Localization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Shared.Caching;

namespace SimplCommerce.Module.Localization
{
    public class EfStringLocalizerFactory : IStringLocalizerFactory
    {
        private ICacheService _cacheService;
        private readonly IServiceProvider _serviceProvider;

        public EfStringLocalizerFactory(IServiceProvider serviceProvider, ICacheService cacheService)
        {
            _serviceProvider = serviceProvider;
            _cacheService = cacheService;
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            return new EfStringLocalizer( _cacheService, _serviceProvider);
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            return new EfStringLocalizer(_cacheService, _serviceProvider);
        }
    }
}