namespace Eshop.Module.Pricing.Services;

using Eshop.Module.Pricing.Contract.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

public class PricingStrategyFactory
{
    private readonly IServiceProvider _serviceProvider;

    public PricingStrategyFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public IPricingStrategy CreateStrategy(string strategyName)
    {
        if (string.IsNullOrWhiteSpace(strategyName))
        {
            throw new ArgumentNullException(nameof(strategyName));
        }

        var key = strategyName.ToLowerInvariant();
        var strategy = _serviceProvider.GetKeyedService<IPricingStrategy>(key);

        return strategy ?? throw new ArgumentException($"Strategy not found for key: {key}");
    }
}