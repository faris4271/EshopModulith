namespace Eshop.Module.Pricing.Contract.Dtos;

public record CalculatedProductPrice(
    decimal CalculatedPrice,
    decimal? OldPrice,
    int PercentOfSaving,
    string PriceText,
    string? OldPriceText
);