namespace Eshop.Module.Basket.Tests;

using Eshop.Module.Basket.Services;
using Eshop.Module.Pricing.Contract.Dtos;
using Eshop.Module.Pricing.Contract.Services;
using Shared.DDD;
using System;
using System.Collections.Generic;
using Xunit;

public class BasketPricingServiceTests
{
    private readonly BasketPricingService _basketPricingService;

    public BasketPricingServiceTests()
    {
        _basketPricingService = new BasketPricingService(new StubPricingService());
    }

    [Fact]
    public void CalculateItemPrice_ReturnsCalculatedPriceFromPricingService()
    {
        var price = new Money(100m, "USD");

        var result = _basketPricingService.CalculateItemPrice(
            price,
            null,
            null,
            null,
            null,
            1,
            null,
            null,
            "simple");

        Assert.Equal(80m, result);
    }

    [Fact]
    public void ApplyBasketDiscount_TreatsValueAsAbsoluteAmount()
    {
        decimal basketTotal = 100m;
        decimal discountAmount = 10m;

        var result = _basketPricingService.ApplyBasketDiscount(basketTotal, discountAmount);

        Assert.Equal(90m, result);
    }

    private sealed class StubPricingService : IPricingService
    {
        public CalculatedProductPrice CalculatePrice(PricingContext context)
        {
            return new CalculatedProductPrice(80m, 100m, 20, "80.00", "100.00");
        }

        public Money ApplyDiscount(Money price, decimal discountPercentage)
        {
            throw new NotSupportedException();
        }

        public Money CalculateTierPrice(Money price, List<TierPrice> tierPrices, int quantity)
        {
            throw new NotSupportedException();
        }
    }
}
