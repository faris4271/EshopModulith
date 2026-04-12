using Eshop.Module.Core.Data;
using Eshop.Module.Core.Services;
using EShop.Module.Core.Contract.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eshop.Module.Core
{
    public static class CoreModule
    {
        public static IServiceCollection AddCore(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddScoped<ICurrencyService,CurrencyService>();

            services.AddScoped<IEntityService, EntityService>();

            services.AddDbContext<CoreDbContext>(op =>
            {
                op.UseNpgsql(configuration.GetConnectionString("defualt"));
            });

            return services;
        }
    }
}
