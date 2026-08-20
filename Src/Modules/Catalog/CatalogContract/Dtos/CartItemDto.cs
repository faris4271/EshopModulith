using Eshop.Module.Pricing.Contract.Dtos;

namespace CatalogContract.Dtos
{
    public class CartItemDto
    {


        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal ProductOldPrice { get; set; } = 0;
        public decimal Price { get; set; }
        public decimal SpecialPrice { get; set; } = 0;

        public DateTimeOffset? SpecialPriceStart { get; set; }
        public DateTimeOffset? SpecialPriceEnd { get; set; }
        public int ProductStockQuantity { get; set; } = 0;
        public bool ProductStockTrackingIsEnabled { get; set; }
        public bool IsProductAvailabeToOrder { get; set; }
        public string ProductImage { get; set; }

        public List<TierPrice> TierPrices { get; set; }
        public decimal DiscountPercentage { get; set; }
        public int Quantity
        { get; set; }
        public IEnumerable<ProductVariationOptionDto> VariationOptions { get; set; }



    }
}
