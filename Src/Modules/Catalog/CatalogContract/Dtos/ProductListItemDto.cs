namespace CatalogContract.Dtos
{
    public class ProductListItemDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool HasOptions { get; set; }

        public bool IsVisibleIndividually { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public bool IsPublished { get; set; }

        public bool IsFeatured { get; set; }

        public bool IsCallForPricing { get; set; }

        public bool IsAllowToOrder { get; set; }

        public int? StockQuantity { get; set; }
    }
}
