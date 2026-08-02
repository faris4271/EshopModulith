namespace CatalogContract.Dtos
{
    public record DiscountableProductDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }



    }
}
