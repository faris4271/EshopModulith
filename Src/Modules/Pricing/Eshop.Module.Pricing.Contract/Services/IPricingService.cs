namespace Eshop.Module.Pricing.Contract.Services;

using Eshop.Module.Pricing.Contract.Dtos;
using Shared.DDD;
using System.Collections.Generic;

public interface IPricingService
{
    CalculatedProductPrice CalculatePrice(PricingContext context);
    Money ApplyDiscount(Money price, decimal discountPercentage);
    Money CalculateTierPrice(Money price, List<TierPrice> tierPrices, int quantity);
}