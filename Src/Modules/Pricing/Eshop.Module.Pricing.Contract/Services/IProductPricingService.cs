using Eshop.Module.Pricing.Contract.Dtos;
using Shared.DDD;
using System;

namespace Eshop.Module.Pricing.Contract.Services
{
    public interface IProductPricingService
    {



        public CalculatedProductPrice CalculateProductPrice(
          Money price,
          Money? oldPrice,
          Money? specialPrice,
          DateTimeOffset? specialPriceStart,
          DateTimeOffset? specialPriceEnd);
    }
}
