using CatalogContract.Dtos;
using CatalogContract.Services;
using EShop.Module.Core.Contract.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Services
{
    
    internal class ProductPricingService : IProductPricingService
    {
        private readonly ICurrencyService _currencyService;

        public ProductPricingService(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        public CalculatedProductPrice CalculateProductPrice(ProductThumbnail productThumbnail)
        {
            return CalculateProductPrice(productThumbnail.Price, productThumbnail.OldPrice, 
                productThumbnail.SpecialPrice, productThumbnail.SpecialPriceStart, productThumbnail.SpecialPriceEnd);
        }

        public CalculatedProductPrice CalculateProductPrice(ProductDto product)
        {
            return CalculateProductPrice(product.Price,
                product.OldPrice, product.SpecialPrice,
                product.SpecialPriceStart, product.SpecialPriceEnd);
        }

        public CalculatedProductPrice CalculateProductPrice(decimal price, decimal? oldPrice, 
            decimal? specialPrice, DateTimeOffset? specialPriceStart, DateTimeOffset? specialPriceEnd)
        {
            decimal calculatedPrice = price;
            var percentOfSaving = 0;
            var now = DateTimeOffset.UtcNow;

            bool specialActive = specialPrice.HasValue
                && (!specialPriceStart.HasValue || specialPriceStart.Value <= now)
                && (!specialPriceEnd.HasValue || specialPriceEnd.Value >= now);

            if (specialActive)
            {
                calculatedPrice = specialPrice.Value;

                if (!oldPrice.HasValue || oldPrice.Value < price)
                {
                    oldPrice = price;
                }
            }

            if (oldPrice.HasValue && oldPrice.Value > 0 && oldPrice.Value > calculatedPrice)
            {
                percentOfSaving = (int)((oldPrice.Value - calculatedPrice) / oldPrice.Value * 100);
            }

            return new CalculatedProductPrice(_currencyService)
            {
                Price = calculatedPrice,
                OldPrice = oldPrice,
                PercentOfSaving = percentOfSaving,
            };
        }
    }
}
