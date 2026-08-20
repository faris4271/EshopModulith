namespace Eshop.Module.Basket;

using Eshop.Module.Basket.Services;
using Microsoft.Extensions.DependencyInjection;

public static class BasketExtensions
{
    public static IServiceCollection AddBasket(this IServiceCollection services)
    {


        // Add Basket services
        services.AddScoped<BasketPricingService>();

        return services;
    }
}