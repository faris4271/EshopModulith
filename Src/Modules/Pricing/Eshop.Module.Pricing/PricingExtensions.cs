using Eshop.Module.Pricing;
using Eshop.Module.Pricing.Contract.Services;
using Eshop.Module.Pricing.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class PricingExtensions
{
    public static IServiceCollection AddPricing(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PricingDbContext>(option =>
        {
            option.UseNpgsql(configuration["Defualt"]);
        });
        services.Configure<ExchangeRateApiOptions>(

        configuration.GetSection(ExchangeRateApiOptions.SectionName));

        services.AddHttpClient<IExchangeRateService, ExchangeRateService>();

        services.AddScoped<ICouponService, CouponService>();

        services.AddHostedService<ExchangeRateSyncJob>();
        return services;
    }
}