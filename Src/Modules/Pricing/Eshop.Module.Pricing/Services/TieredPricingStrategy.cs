namespace Eshop.Module.Pricing.Services;

using Eshop.Module.Pricing.Contract.Dtos;
using Eshop.Module.Pricing.Contract.Services;
using Shared.DDD;

public class TieredPricingStrategy : IPricingStrategy
{
    public CalculatedProductPrice CalculatePrice(Money price, Money? oldPrice, Money? specialPrice,
        DateTimeOffset? specialPriceStart, DateTimeOffset? specialPriceEnd,
        int? quantity = null, List<TierPrice>? tierPrices = null, decimal? discountPercentage = null)
    {
        var calculatedPrice = price;
        var percentOfSaving = 0;
        var now = DateTimeOffset.UtcNow;

        bool isSpecialActive = specialPrice is not null
            && (!specialPriceStart.HasValue || specialPriceStart.Value <= now)
            && (!specialPriceEnd.HasValue || specialPriceEnd.Value >= now);

        if (isSpecialActive)
        {
            calculatedPrice = specialPrice!;

            if (oldPrice is null || oldPrice.Amount < price.Amount)
            {
                oldPrice = price;
            }
        }

        if (tierPrices is not null && quantity.HasValue)
        {
            var applicableTier = tierPrices.OrderByDescending(t => t.Quantity).FirstOrDefault(t => quantity >= t.Quantity);

            if (applicableTier is not null)
            {
                calculatedPrice = new Money(applicableTier.Price, calculatedPrice.Currency);
            }
        }

        if (oldPrice is not null && oldPrice.Amount > 0m && oldPrice.Amount > calculatedPrice.Amount)
        {
            percentOfSaving = (int)((oldPrice.Amount - calculatedPrice.Amount) / oldPrice.Amount * 100m);
        }

        return new CalculatedProductPrice(
            CalculatedPrice: calculatedPrice.Amount,
            OldPrice: oldPrice?.Amount,
            PercentOfSaving: percentOfSaving,
            PriceText: calculatedPrice.ToString(),
            OldPriceText: oldPrice?.ToString()
        );
    }
}