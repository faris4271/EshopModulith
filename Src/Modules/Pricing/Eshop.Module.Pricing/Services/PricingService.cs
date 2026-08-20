namespace Eshop.Module.Pricing.Services;

using Eshop.Module.Pricing.Contract.Dtos;
using Eshop.Module.Pricing.Contract.Services;
using Shared.DDD;
using System;
using System.Collections.Generic;
using System.Linq;

public class PricingService : IPricingService
{
    private readonly ICurrencyService _currencyService;
    private readonly PricingStrategyFactory _strategyFactory;

    public PricingService(ICurrencyService currencyService, PricingStrategyFactory strategyFactory)
    {
        _currencyService = currencyService ?? throw new ArgumentNullException(nameof(currencyService));
        _strategyFactory = strategyFactory ?? throw new ArgumentNullException(nameof(strategyFactory));
    }

    public CalculatedProductPrice CalculatePrice(Contract.Services.PricingContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        // Determine the appropriate strategy based on the context
        var strategy = _strategyFactory.CreateStrategy(context.StrategyName);

        // Calculate the price using the selected strategy
        var result = strategy.CalculatePrice(
            context.Price,
            context.OldPrice,
            context.SpecialPrice,
            context.SpecialPriceStart,
            context.SpecialPriceEnd,
            context.Quantity,
            context.TierPrices,
            context.DiscountPercentage
        );

        // Format the price text using the CurrencyService
        var priceText = _currencyService.FormatCurrency(new Money(result.CalculatedPrice, context.Price.Currency));
        var oldPriceText = result.OldPrice.HasValue
            ? _currencyService.FormatCurrency(new Money(result.OldPrice.Value, context.Price.Currency))
            : null;

        return new CalculatedProductPrice(
            CalculatedPrice: result.CalculatedPrice,
            OldPrice: result.OldPrice,
            PercentOfSaving: result.PercentOfSaving,
            PriceText: priceText,
            OldPriceText: oldPriceText
        );
    }

    public Money ApplyDiscount(Money price, decimal discountPercentage)
    {
        ArgumentNullException.ThrowIfNull(price);

        var discountAmount = price.Amount * (discountPercentage / 100m);
        return new Money(price.Amount - discountAmount, price.Currency);
    }

    public Money CalculateTierPrice(Money price, List<TierPrice> tierPrices, int quantity)
    {
        ArgumentNullException.ThrowIfNull(price);

        if (tierPrices is null || tierPrices.Count == 0)
        {
            return price;
        }

        var applicableTier = tierPrices
            .Where(t => quantity >= t.Quantity)
            .OrderByDescending(t => t.Quantity)
            .FirstOrDefault();

        return applicableTier is not null
            ? new Money(applicableTier.Price, price.Currency)
            : price;
    }


}