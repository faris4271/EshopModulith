using Eshop.Module.Pricing.Contract.Dtos;
using Shared.DDD;
using System;
using System.Collections.Generic;


namespace Eshop.Module.Pricing.Contract.Services;

public interface IPricingStrategy
{
    CalculatedProductPrice CalculatePrice(Money price, Money? oldPrice, Money? specialPrice,
        DateTimeOffset? specialPriceStart, DateTimeOffset? specialPriceEnd,
        int? quantity = null, List<TierPrice>? tierPrices = null, decimal? discountPercentage = null);
}