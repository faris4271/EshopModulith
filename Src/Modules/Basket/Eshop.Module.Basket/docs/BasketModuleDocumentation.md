# Basket Module Documentation

## Overview

The Basket Module handles all operations related to the shopping basket functionality in the EshopModulith application. It provides services for adding items to the basket, calculating the basket total, and applying discounts.

## Key Features

1. Add items to the basket with calculated prices
2. Calculate the total price of the basket
3. Apply discounts to the basket
4. Manage basket items and quantities

## Integration with Pricing Module

The Basket Module is integrated with the Pricing Module to calculate final prices for items in the basket. The Pricing Module provides flexible pricing calculations that can be used to determine the final price of items in the basket and shipping costs.

### BasketPricingService

The `BasketPricingService` is a dedicated service within the Basket Module that handles all pricing-related operations. It interacts with the Pricing Module to calculate the final price of items in the basket and apply any applicable discounts.

#### Key Features

1. **Item Price Calculation**: Calculates the final price of an item in the basket, taking into account special prices, discounts, and quantity.
2. **Basket Discount Application**: Applies basket-level discounts to the total price of the basket.
3. **Price Formatting**: Formats prices for display in the user interface.

#### Usage

The `BasketPricingService` is used by the `BasketService` to calculate the final price of items in the basket and apply any applicable discounts. It is also used by the `BasketController` to return the basket total to the client.

#### Example

```csharp
var finalPrice = await _basketPricingService.CalculateItemPriceAsync(
    productPrice,
    productSpecialPrice,
    specialPriceStart,
    specialPriceEnd,
    quantity,
    productDiscount,
    discountPercentage,
    pricingStrategy
);
```

The `BasketPricingService` is registered in the DI container using the `AddBasketPricing` extension method, which is called by the `AddBasket` extension method in the `BasketExtensions` class.

## Services

1. **BasketService**: Main service for basket operations
   - AddToBasketAsync: Adds an item to the basket
   - CalculateBasketTotalAsync: Calculates the total price of the basket
   - UpdateBasketItemAsync: Updates an item in the basket
   - RemoveBasketItemAsync: Removes an item from the basket

2. **BasketPricingService**: Service for pricing calculations
   - CalculateItemPriceAsync: Calculates the final price of an item
   - ApplyBasketDiscountAsync: Applies a discount to the basket total

## Models

1. **BasketItem**: Represents an item in the basket
   - Properties: Id, ProductId, Quantity, Price, OriginalPrice, PriceText, OriginalPriceText, CreatedAt, UpdatedAt

2. **Basket**: Represents the shopping basket
   - Properties: Id, UserId, Items, CreatedAt, UpdatedAt

## DTOs

1. **AddToCartResult**: Result of adding an item to the basket
   - Properties: Success, Message, BasketItem

2. **BasketItemDto**: Data transfer object for basket items
   - Properties: Id, ProductId, Quantity, Price, OriginalPrice, PriceText, OriginalPriceText, CreatedAt, UpdatedAt

## Best Practices

1. **Use the Money Value Object**: Always use the Money value object for monetary values to ensure proper currency handling and calculations.
2. **Cache Pricing Calculations**: Consider caching frequently used pricing calculations to improve performance.
3. **Error Handling**: Implement proper error handling for pricing calculations to ensure a smooth user experience.
4. **Logging**: Log pricing activities for auditing and debugging purposes.

## Troubleshooting

If you encounter issues with the Basket Module:

1. **Verify Service Registration**: Ensure that the Pricing Module is properly registered in the DI container.
2. **Check Pricing Context**: Verify that the PricingContext is properly initialized with all required information.
3. **Review Logs**: Check the logs for any pricing-related errors or warnings.
4. **Test with Simple Scenarios**: Start with simple pricing scenarios and gradually increase complexity to identify issues.
5. **Consult Documentation**: Refer to the Pricing Module documentation for more detailed information.

## Usage Examples

### Adding an Item to the Basket

```csharp
var result = await _basketService.AddToBasketAsync(productId, quantity);
if (result.Success)
{
    // Item added successfully
}
```

### Calculating the Basket Total

```csharp
var total = await _basketService.CalculateBasketTotalAsync(basketId);
```

### Applying a Discount to the Basket

```csharp
var discountedTotal = await _basketPricingService.ApplyBasketDiscountAsync(basketTotal, discountPercentage);
```

This documentation provides a comprehensive guide for using the Basket Module in the EshopModulith application. For more detailed information, refer to the source code and comments in the module.