using Catalog.Data;
using Catalog.Services;
using CatalogContract.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Abstraction;
using Shared.Data;
using Shared.Data.interceptores;
using Shared.Eventing;
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

            service.AddScoped<ISaveChangesInterceptor, DispachDomainEventInterceptor>();
            service.AddScoped(typeof(IGenericeRepository<,>), typeof(GenericeRepository<,>));
            //eventing 
            service.AddEventingForDbContext<CatalogDbContext>();
            service.AddIntegrationEventHandlers(typeof(CatalogDbContext).Assembly);

            service.AddScoped<DispachDomainEventInterceptor>();
            service.AddScoped<AuditableEntityInterceptor>();
            service.AddScoped<IProductPricingService, ProductPricingService>();
            service.AddDbContext<CatalogDbContext>((sp, option) =>
            {
                var domainEventInterceptor = sp.GetRequiredService<DispachDomainEventInterceptor>();
                var auditInterceptor = sp.GetRequiredService<AuditableEntityInterceptor>();
                option.AddInterceptors(domainEventInterceptor).AddInterceptors(auditInterceptor);


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

