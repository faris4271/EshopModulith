namespace Eshop.Module.Pricing.Contract.Services;

using Eshop.Module.Pricing.Contract.Dtos;
using Shared.DDD;
using System;
using System.Collections.Generic;

public class PricingContext
{
    public Money Price { get; set; }
    public Money? OldPrice { get; set; }
    public Money? SpecialPrice { get; set; }
    public DateTimeOffset? SpecialPriceStart { get; set; }
    public DateTimeOffset? SpecialPriceEnd { get; set; }
    public int? Quantity { get; set; }
    public List<TierPrice>? TierPrices { get; set; }
    public decimal? DiscountPercentage { get; set; }
    public string StrategyName { get; set; } = "simple";

    public PricingContext(Money price, Money? oldPrice = null, Money? specialPrice = null,
        DateTimeOffset? specialPriceStart = null, DateTimeOffset? specialPriceEnd = null,
        int? quantity = null, List<TierPrice>? tierPrices = null, decimal? discountPercentage = null,
        string strategyName = "simple")
    {
        Price = price;
        OldPrice = oldPrice;
        SpecialPrice = specialPrice;
        SpecialPriceStart = specialPriceStart;
        SpecialPriceEnd = specialPriceEnd;
        Quantity = quantity;
        TierPrices = tierPrices;
        DiscountPercentage = discountPercentage;
        StrategyName = strategyName;
    }
}