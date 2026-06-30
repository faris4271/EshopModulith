using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Module.Inventory.Data;
using Shared.Data;
using Shared.Eventing;

namespace Module.Inventory
{
    public static class InventoryModule
    {

        public static IServiceCollection AddInventory(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<InventoryDbContext>(options =>
            {
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
