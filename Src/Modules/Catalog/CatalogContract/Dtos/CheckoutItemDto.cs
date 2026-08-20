using Eshop.Module.Pricing.Contract.Dtos;

namespace CatalogContract.Dtos
{
    public record CheckoutItemDto
    {


        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductImage { get; set; }

        public CalculatedProductPrice CalculatedProductPrice { get; set; }

        public decimal ProductPrice { get; set; }

        public string ProductPriceString { get; set; }

        public int ProductStockQuantity { get; set; }

        public bool ProductStockTrackingIsEnabled { get; set; }

        public bool IsProductAvailabeToOrder { get; set; }

        public int Quantity { get; set; }

        public decimal Total => Quantity * ProductPrice;

        public string TotalString { get; set; }

        public IEnumerable<ProductVariationOptionDto> VariationOptions { get; set; } = new List<ProductVariationOptionDto>();


    }
}