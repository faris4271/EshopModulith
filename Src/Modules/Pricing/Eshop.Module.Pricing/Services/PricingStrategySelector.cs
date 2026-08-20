namespace Eshop.Module.Pricing.Services;

using Eshop.Module.Pricing.Contract.Services;

public class PricingStrategySelector
{
    private readonly Dictionary<string, IPricingStrategy> _strategies;

    public PricingStrategySelector()
    {
        _strategies = new Dictionary<string, IPricingStrategy>
        {
            { "simple", new SimplePricingStrategy() },
            { "tiered", new TieredPricingStrategy() },
            { "discount", new DiscountPricingStrategy() },
            { "bulk", new BulkPricingStrategy() }
        };
    }

    public IPricingStrategy GetStrategy(string strategyName)
    {
        if (_strategies.TryGetValue(strategyName.ToLower(), out var strategy))
        {
            return strategy;
        }

        throw new ArgumentException($"Unknown pricing strategy: {strategyName}");
    }
}