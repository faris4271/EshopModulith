namespace CatalogContract.Dtos;

using Shared.DDD;

public record CalculatedProductPrice(
    Money CalculatedPrice,
    Money? OldPrice,
    int PercentOfSaving,
    string PriceText,
    string? OldPriceText
);