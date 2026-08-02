using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Module.Tax.Services;
using Shared.Data;

namespace Module.Tax
{
    public static class TaxModule
    {

        public static IServiceCollection AddTax(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<Tax.Data.TaxDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("defualt"));
            });

            services.AddScoped<ITaxService, TaxService>();

            return services;
        }


        public static IApplicationBuilder UseTax(this IApplicationBuilder app)
        {
            app.UseMigration<Tax.Data.TaxDbContext>();
            return app;
        }
    }
}
