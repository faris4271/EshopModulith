using CatalogContract.Dtos;
using Shared.DDD;


namespace CatalogContract.Services
{
    public interface IProductPricingService
    {

        CalculatedProductPrice CalculateProductPrice(CartItemsDto product);

        public CalculatedProductPrice CalculateProductPrice(
          Money price,
          Money? oldPrice,
          Money? specialPrice,
          DateTimeOffset? specialPriceStart,
          DateTimeOffset? specialPriceEnd);
    }
}
