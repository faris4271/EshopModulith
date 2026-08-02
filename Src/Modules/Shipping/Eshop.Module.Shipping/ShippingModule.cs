using Eshop.Module.Shipping.Data;
using Eshop.Module.Shipping.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Data;

namespace Eshop.Module.Shipping
{
    public static class ShippingModule
    {
        public static IServiceCollection AddShipping(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ShippingDbContext>(option =>
            {
                option.UseNpgsql(configuration["defualt"]);
            });
            services.AddScoped<IShippingPriceService, ShippingPriceService>();

            return services;
        }

        public static IApplicationBuilder UseShipping(this IApplicationBuilder app)
        {

            app.UseMigration<ShippingDbContext>();

            return app;

        }
    }
}
