

using EShop.Module.Core.Contract.Dtos;

namespace CatalogContract.Dtos
{
    public class ProductDetailVariation
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string NormalizedName { get; set; }

        public bool IsCallForPricing { get; set; }

        public bool IsAllowToOrder { get; set; }

        public int StockQuantity { get; set; }

        public bool StockTrackingIsEnabled { get; set; }

        public CalculatedProductPrice CalculatedProductPrice { get; set; }

        public IList<MediaDto> Images { get; set; } = new List<MediaDto>();

        public IList<ProductDetailVariationOption> Options { get; protected set; } = new List<ProductDetailVariationOption>();
    }
}
