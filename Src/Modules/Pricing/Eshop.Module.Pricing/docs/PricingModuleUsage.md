# Pricing Module Usage Guide

## Overview

The Pricing Module provides a flexible and extensible pricing system for the EshopModulith application. It implements a strategy pattern for different pricing scenarios and supports various pricing rules including special prices, discounts, and tiered pricing.

## Key Components

1. **IPricingService**: The main interface for pricing operations
2. **IPricingStrategy**: Interface for different pricing strategies
3. **PricingContext**: Encapsulates all pricing information
4. **PricingStrategyFactory**: Creates the appropriate pricing strategy
5. **PricingExtensions**: Extension methods for DI registration

## Getting Started

### 1. Register the Pricing Module

Add the pricing module to your DI container in the `Program.cs` or `Startup.cs` file:

```csharp
builder.Services.AddPricing();
```

### 2. Inject IPricingService

Inject the `IPricingService` into your services or controllers:

```csharp
public class ProductService
{
    private readonly IPricingService _pricingService;

    public ProductService(IPricingService pricingService)
    {
        _pricingService = pricingService;
    }

    // Use the pricing service methods
}
```

## Using the Pricing Service

### Calculating Product Price

To calculate a product price, create a `PricingContext` and call the `CalculatePrice` method:

```csharp
var context = new PricingContext(
    price: new Money(100m, "USD"),
    oldPrice: new Money(120m, "USD"),
    specialPrice: new Money(80m, "USD"),
    specialPriceStart: DateTimeOffset.UtcNow.AddDays(-1),
    specialPriceEnd: DateTimeOffset.UtcNow.AddDays(1),
    quantity: 2,
    tierPrices: new List<TierPrice> { new TierPrice(5, 75m), new TierPrice(10, 70m) },
    discountPercentage: 10m,
    strategyName: "bulk"
);

var calculatedPrice = _pricingService.CalculatePrice(context);
```

### Applying Discounts

To apply a discount to a price:

```csharp
var originalPrice = new Money(100m, "USD");
var discountedPrice = _pricingService.ApplyDiscount(originalPrice, 10m); // 10% discount
```

### Calculating Tiered Price

To calculate a tiered price based on quantity:

```csharp
var basePrice = new Money(100m, "USD");
var tierPrices = new List<TierPrice> { new TierPrice(5, 90m), new TierPrice(10, 85m) };
var tieredPrice = _pricingService.CalculateTierPrice(basePrice, tierPrices, 7); // Returns 90m for quantity 7
```

## Pricing Strategies

The pricing module supports several strategies:

1. **Simple**: Basic pricing without any special considerations
2. **Tiered**: Pricing based on quantity thresholds
3. **Discount**: Pricing with percentage discounts
4. **Bulk**: Combination of tiered pricing and discounts

To use a specific strategy, set the `strategyName` property in the `PricingContext`.

## Creating Custom Strategies

To create a custom pricing strategy:

1. Implement the `IPricingStrategy` interface
2. Register your strategy in the DI container with a key matching your strategy name

```csharp
// Example custom strategy
public class CustomPricingStrategy : IPricingStrategy
{
    public CalculatedProductPrice CalculatePrice(Money price, Money? oldPrice, Money? specialPrice,
        DateTimeOffset? specialPriceStart, DateTimeOffset? specialPriceEnd,
        int? quantity = null, List<TierPrice>? tierPrices = null, decimal? discountPercentage = null)
    {
        // Implement your custom pricing logic
        // ...
    }
}

// Register in DI
services.AddKeyedScoped<IPricingStrategy, CustomPricingStrategy>("custom");
```

## Pricing Context Properties

The `PricingContext` class contains the following properties:

- `Price`: The base price of the product
- `OldPrice`: The previous price of the product
- `SpecialPrice`: The special price of the product
- `SpecialPriceStart`: The start date of the special price
- `SpecialPriceEnd`: The end date of the special price
- `Quantity`: The quantity of the product being purchased
- `TierPrices`: List of tiered pricing rules
- `DiscountPercentage`: The discount percentage to apply
- `StrategyName`: The name of the pricing strategy to use

## Calculated Product Price

The `CalculatedProductPrice` record contains the following properties:

- `CalculatedPrice`: The final calculated price
- `OldPrice`: The original price before any calculations
- `PercentOfSaving`: The percentage of savings compared to the original price
- `PriceText`: The formatted price text
- `OldPriceText`: The formatted original price text

## Tier Price

The `TierPrice` record contains the following properties:

- `Quantity`: The quantity threshold for this tier
- `Price`: The price for this tier

## Best Practices

1. Always use the `Money` value object for monetary values
2. Set appropriate strategy names based on your pricing requirements
3. Provide all relevant information in the `PricingContext` for accurate calculations
4. Consider caching frequently used pricing calculations
5. Test your pricing logic thoroughly with various scenarios

## Troubleshooting

If you encounter issues with the pricing module:

1. Verify that all required services are properly registered in the DI container
2. Check that you're using the correct strategy names
3. Ensure that all monetary values are properly initialized with the correct currency
4. Review the pricing context properties to ensure all required information is provided
5. Check the logs for any pricing-related errors or warnings

This documentation provides a comprehensive guide for using the Pricing Module in the EshopModulith application. For more detailed information, refer to the source code and comments in the module.