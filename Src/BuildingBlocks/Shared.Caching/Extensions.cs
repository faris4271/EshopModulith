using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Caching
{
    public static class Extensions
    {
        public static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(configuration);

            services.AddOptions<CachingOptions>().BindConfiguration(nameof(CachingOptions));

            services.AddMemoryCache();
            var cachConfig = configuration.GetSection(nameof(CachingOptions)).Get<CachingOptions>();
            if (cachConfig.Redis is null || string.IsNullOrEmpty(cachConfig.Redis))
            {
                services.AddDistributedMemoryCache();
                services.AddTransient<ICacheService, HybridCacheService>();

                return services;
            }

            services.AddStackExchangeRedisCache(options =>
            {
                var config = ConfigurationOptions.Parse(cachConfig.Redis);
                config.AbortOnConnectFail = true;

                // Only override SSL if explicitly configured
                if (cachConfig.EnableSsl.HasValue)
                {
                    config.Ssl = cachConfig.EnableSsl.Value;
                }

                options.ConfigurationOptions = config;
            });

            services.AddTransient<ICacheService, HybridCacheService>();

            return services;
        }
    }
}
