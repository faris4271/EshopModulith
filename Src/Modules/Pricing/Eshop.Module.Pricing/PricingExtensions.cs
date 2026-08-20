namespace Eshop.Module.Pricing;

using Eshop.Module.Pricing.Contract.Services;
using Eshop.Module.Pricing.Data;
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
            option.UseNpgsql(configuration.GetConnectionString("defualt"));
        });

        services.AddScoped<IPricingService, PricingService>();
        services.AddScoped<ICouponService, CouponService>();
        services.AddScoped<PricingStrategyFactory>();
        services.AddScoped<ICurrencyService, CurrencyService>();

        // Register all pricing strategies as keyed services for PricingStrategyFactory
        services.AddKeyedScoped<IPricingStrategy, SimplePricingStrategy>("simple");
        services.AddKeyedScoped<IPricingStrategy, TieredPricingStrategy>("tiered");
        services.AddKeyedScoped<IPricingStrategy, DiscountPricingStrategy>("discount");
        services.AddKeyedScoped<IPricingStrategy, BulkPricingStrategy>("bulk");

        return services;
    }
}