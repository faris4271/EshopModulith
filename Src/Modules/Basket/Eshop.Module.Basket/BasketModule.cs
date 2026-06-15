using Eshop.Module.Basket.Contract.Services;
using Eshop.Module.Basket.Data;
using Eshop.Module.Basket.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Data;

namespace Eshop.Module.Basket
{
    public static class BasketModule
    {
        public static IServiceCollection AddBasket(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BasketDbContext>(op =>
            {
                op.UseNpgsql(configuration.GetConnectionString("defualt"));
            });

            services.AddScoped<ICartService, CartService>();

            services.AddScoped<ICouponService, CouponService>();


            return services;
        }

        public static IApplicationBuilder UseBasket(this IApplicationBuilder app)
        {
            app.UseMigration<BasketDbContext>();
            return app;
        }
    }
}
