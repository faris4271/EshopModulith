using Eshop.Module.Core.Data;
using Eshop.Module.Core.Services;
using EShop.Module.Core.Contract.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Abstraction;
using Shared.Data;
using System.Reflection;

namespace Eshop.Module.Core
{
    public static class CoreModule
    {
        public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
        {


            services.AddScoped<IEntityService, EntityService>();

            services.AddScoped(typeof(IGenericeRepository<,>), typeof(GenericeRepository<,>));

            services.AddDbContext<CoreDbContext>(op =>
            {
                op.UseNpgsql(configuration.GetConnectionString("defualt"));
            });

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

            });
            return services;
        }

        public static IApplicationBuilder UseCore(this IApplicationBuilder app)
        {

            app.UseMigration<CoreDbContext>();

            return app;
        }
    }
}
