using Catalog.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Abstraction;
using Shared.Data;
using Shared.Data.interceptores;
using Shared.Services;
using System.Reflection;

namespace Catalog
{
    public static class CatalogModule
    {
        public static IServiceCollection AddCatalog(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            });

            service.AddScoped<ISaveChangesInterceptor, AuditableSaveChangeInterceptor>();
            service.AddScoped<ISaveChangesInterceptor, DispachDomainEventInterceptor>();
            service.AddScoped(typeof(IGenericeRepository<,>), typeof(GenericeRepository<,>));

            service.AddScoped<IFileService, LocalFileService>();




            service.AddDbContext<CatalogDbContext>((sp, option) =>
            {
                option.AddInterceptors(sp.GetService<ISaveChangesInterceptor>());

                option.UseNpgsql(configuration.GetConnectionString("defualt"));
            });
            return service;
        }


        public static IApplicationBuilder UseCatalog(this IApplicationBuilder app)
        {

            app.UseMigration<CatalogDbContext>();

            return app;

        }


    }
}

