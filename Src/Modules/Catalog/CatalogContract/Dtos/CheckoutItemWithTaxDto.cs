namespace CatalogContract.Dtos
{
    public record CheckoutItemWithTaxDto
    {
        public Guid productId { get; set; }
        public decimal productPrice { get; set; }
        public Guid? taxClassId { get; set; }

        public int quantity { get; set; }

    };

}
