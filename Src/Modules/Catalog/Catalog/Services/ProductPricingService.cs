namespace Catalog.Services;

using Eshop.Module.Pricing.Contract.Dtos;
using Eshop.Module.Pricing.Contract.Services;
using Shared.DDD;
using System;

internal class ProductPricingService : IProductPricingService
{
    private readonly ICurrencyService _currencyService;

    public ProductPricingService(ICurrencyService currencyService)
    {
        _currencyService = currencyService ?? throw new ArgumentNullException(nameof(currencyService));
    }




    public CalculatedProductPrice CalculateProductPrice(
        Money price,
        Money? oldPrice,
        Money? specialPrice,
        DateTimeOffset? specialPriceStart,
        DateTimeOffset? specialPriceEnd)
    {
        ArgumentNullException.ThrowIfNull(price);

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

        if (oldPrice is not null && oldPrice.Amount > 0m && oldPrice.Amount > calculatedPrice.Amount)
        {
            percentOfSaving = (int)((oldPrice.Amount - calculatedPrice.Amount) / oldPrice.Amount * 100m);
        }


        return new CalculatedProductPrice(
            CalculatedPrice: calculatedPrice.Amount,
            OldPrice: oldPrice?.Amount,
            PercentOfSaving: percentOfSaving,
            PriceText: _currencyService.FormatCurrency(calculatedPrice),
            OldPriceText: oldPrice is not null ? _currencyService.FormatCurrency(oldPrice) : null
        );
    }


}