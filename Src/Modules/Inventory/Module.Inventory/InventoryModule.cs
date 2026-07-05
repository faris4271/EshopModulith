using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Module.Inventory.Data;
using Module.Inventory.Services;
using Shared.Abstraction;
using Shared.Data;
using Shared.Data.interceptores;
using Shared.Eventing;
using System.Reflection;

namespace Module.Inventory
{
    public static class InventoryModule
    {
        public static IServiceCollection AddInventory(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            });

            services.AddScoped<ISaveChangesInterceptor, DispachDomainEventInterceptor>();
            services.AddScoped(typeof(IGenericeRepository<,>), typeof(GenericeRepository<,>));
            services.AddScoped<IStockService, StockService>();

            services.AddScoped<DispachDomainEventInterceptor>();
            services.AddScoped<AuditableEntityInterceptor>();

            services.AddDbContext<InventoryDbContext>((sp, options) =>
            {
                var domainEventInterceptor = sp.GetRequiredService<DispachDomainEventInterceptor>();
                var auditInterceptor = sp.GetRequiredService<AuditableEntityInterceptor>();
                options.AddInterceptors(domainEventInterceptor).AddInterceptors(auditInterceptor);

                options.UseNpgsql(configuration.GetConnectionString("defualt"));
            });

            services.AddEventingForDbContext<InventoryDbContext>();
            services.AddIntegrationEventHandlers(typeof(InventoryModule).Assembly);
            return services;
        }

        public static IApplicationBuilder UseInventory(this IApplicationBuilder app)
        {
            app.UseMigration<InventoryDbContext>();
            return app;
        }
    }
}
