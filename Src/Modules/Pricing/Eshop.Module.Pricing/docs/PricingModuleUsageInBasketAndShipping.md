# Using the Pricing Module in Basket and Shipping Modules

## Overview

This guide explains how to integrate and use the Pricing Module in the Basket and Shipping modules of the EshopModulith application. The Pricing Module provides flexible pricing calculations that can be used to determine the final price of items in the basket and shipping costs.

## Prerequisites

Before using the Pricing Module in the Basket and Shipping modules, ensure that:

1. The Pricing Module is properly registered in the DI container
2. The necessary services are injected into the relevant classes
3. The Money Value object is used for all monetary calculations

## Using the Pricing Module in the Basket Module

### 1. Inject IPricingService

Inject the IPricingService into your BasketService or relevant classes:

```csharp
public class BasketService
{
    private readonly IPricingService _pricingService;

    public BasketService(IPricingService pricingService)
    {
        _pricingService = pricingService;
    }

    // Use the pricing service methods
}
```

### 2. Calculate Product Prices in the Basket

When adding items to the basket or calculating the total, use the Pricing Module to calculate the final price of each product:

```csharp
public async Task AddToBasketAsync(Guid productId, int quantity)
{
    // Get product details from the database
    var product = await _productRepository.GetByIdAsync(productId);

    // Create pricing context
    var context = new PricingContext(
        price: new Money(product.Price, "USD"),
        oldPrice: product.OldPrice.HasValue ? new Money(product.OldPrice.Value, "USD") : null,
        specialPrice: product.SpecialPrice.HasValue ? new Money(product.SpecialPrice.Value, "USD") : null,
        specialPriceStart: product.SpecialPriceStart,
        specialPriceEnd: product.SpecialPriceEnd,
        quantity: quantity,
        tierPrices: product.TierPrices?.Select(t => new TierPrice(t.Quantity, t.Price)).ToList(),
        discountPercentage: product.DiscountPercentage,
        strategyName: "bulk"
    );

    // Calculate the final price
    var calculatedPrice = _pricingService.CalculatePrice(context);

    // Add the item to the basket with the calculated price
    var basketItem = new BasketItem
    {
        ProductId = productId,
        Quantity = quantity,
        Price = calculatedPrice.CalculatedPrice,
        OriginalPrice = calculatedPrice.OldPrice ?? calculatedPrice.CalculatedPrice,
        PriceText = calculatedPrice.PriceText,
        OriginalPriceText = calculatedPrice.OldPriceText ?? calculatedPrice.PriceText
    };

    // Add the item to the basket
    _basketRepository.AddItem(basketItem);
}
```

### 3. Calculate Basket Total

When calculating the total price of the basket, use the Pricing Module to apply any additional discounts or fees:

```csharp
public async Task<decimal> CalculateBasketTotalAsync(Guid basketId)
{
    var basket = await _basketRepository.GetByIdAsync(basketId);
    decimal total = 0m;

    foreach (var item in basket.Items)
    {
        // Calculate the total for each item
        total += item.Price * item.Quantity;
    }

    // Apply any basket-level discounts
    if (basket.DiscountCode != null)
    {
        var discount = await _discountService.GetDiscountByCodeAsync(basket.DiscountCode);
        if (discount != null)
        {
            total = _pricingService.ApplyDiscount(new Money(total, "USD"), discount.Percentage).Amount;
        }
    }

    return total;
}
```

## Using the Pricing Module in the Shipping Module

### 1. Inject IPricingService

Inject the IPricingService into your ShippingService or relevant classes:

```csharp
public class ShippingService
{
    private readonly IPricingService _pricingService;

    public ShippingService(IPricingService pricingService)
    {
        _pricingService = pricingService;
    }

    // Use the pricing service methods
}
```

### 2. Calculate Shipping Costs

Use the Pricing Module to calculate shipping costs based on the basket total and other factors:

```csharp
public async Task<decimal> CalculateShippingCostAsync(Guid basketId)
{
    var basket = await _basketRepository.GetByIdAsync(basketId);
    var basketTotal = await _basketService.CalculateBasketTotalAsync(basketId);

    // Determine the shipping method based on the basket total
    var shippingMethod = await _shippingMethodRepository.GetByBasketTotalAsync(basketTotal);

    // Create pricing context for shipping
    var context = new PricingContext(
        price: new Money(shippingMethod.BaseCost, "USD"),
        discountPercentage: shippingMethod.DiscountPercentage,
        strategyName: "discount"
    );

    // Calculate the final shipping cost
    var calculatedShippingCost = _pricingService.CalculatePrice(context);

    return calculatedShippingCost.CalculatedPrice;
}
```

### 3. Apply Shipping Discounts

Apply shipping discounts based on the basket total or other criteria:

```csharp
public async Task<decimal> ApplyShippingDiscountAsync(Guid basketId, string discountCode)
{
    var basket = await _basketRepository.GetByIdAsync(basketId);
    var shippingCost = await CalculateShippingCostAsync(basketId);

    // Get the discount for the shipping
    var discount = await _discountService.GetDiscountByCodeAsync(discountCode);

    if (discount != null && discount.AppliesToShipping)
    {
        // Apply the discount to the shipping cost
        var discountedShippingCost = _pricingService.ApplyDiscount(new Money(shippingCost, "USD"), discount.Percentage);

        return discountedShippingCost.Amount;
    }

    return shippingCost;
}
```

## Best Practices

1. **Use the Money Value Object**: Always use the Money value object for monetary values to ensure proper currency handling and calculations.
2. **Cache Pricing Calculations**: Consider caching frequently used pricing calculations to improve performance.
3. **Test Thoroughly**: Test your pricing logic with various scenarios to ensure accuracy and reliability.
4. **Handle Errors Gracefully**: Implement proper error handling for pricing calculations to ensure a smooth user experience.
5. **Log Pricing Activities**: Log pricing activities for auditing and debugging purposes.

## Troubleshooting

If you encounter issues with the Pricing Module in the Basket or Shipping modules:

1. **Verify Service Registration**: Ensure that the Pricing Module is properly registered in the DI container.
2. **Check Pricing Context**: Verify that the PricingContext is properly initialized with all required information.
3. **Review Logs**: Check the logs for any pricing-related errors or warnings.
4. **Test with Simple Scenarios**: Start with simple pricing scenarios and gradually increase complexity to identify issues.
5. **Consult Documentation**: Refer to the Pricing Module documentation for more detailed information.

This guide provides comprehensive instructions for using the Pricing Module in the Basket and Shipping modules of the EshopModulith application. For more detailed information, refer to the source code and comments in the module.