namespace Eshop.Module.Basket.Services;

using Eshop.Module.Pricing.Contract.Dtos;
using Eshop.Module.Pricing.Contract.Services;
using Shared.DDD;
using System;
using System.Collections.Generic;

public class BasketPricingService
{
    private readonly IPricingService _pricingService;

    public BasketPricingService(IPricingService pricingService)
    {
        _pricingService = pricingService ?? throw new ArgumentNullException(nameof(pricingService));
    }

    public decimal CalculateItemPrice(Money price, Money? oldPrice, Money? specialPrice,
        DateTimeOffset? specialPriceStart, DateTimeOffset? specialPriceEnd, int quantity,
        List<TierPrice>? tierPrices, decimal? discountPercentage, string strategyName = "bulk")
    {
        var context = new PricingContext(
            price: price,
            oldPrice: oldPrice,
            specialPrice: specialPrice,
            specialPriceStart: specialPriceStart,
            specialPriceEnd: specialPriceEnd,
            quantity: quantity,
            tierPrices: tierPrices,
            discountPercentage: discountPercentage,
            strategyName: strategyName
        );

        var calculatedPrice = _pricingService.CalculatePrice(context);
        return calculatedPrice.CalculatedPrice;
    }

    public decimal ApplyBasketDiscount(decimal basketTotal, decimal discountAmount)
    {
        var discountedTotal = basketTotal - discountAmount;
        return discountedTotal < 0m ? 0m : discountedTotal;
    }
}