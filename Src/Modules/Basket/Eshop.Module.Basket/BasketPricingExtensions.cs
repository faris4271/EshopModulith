namespace Eshop.Module.Basket;

using Eshop.Module.Basket.Services;
using Microsoft.Extensions.DependencyInjection;

public static class BasketPricingExtensions
{
    public static IServiceCollection AddBasketPricing(this IServiceCollection services)
    {
        services.AddScoped<BasketPricingService>();
        return services;
    }
}