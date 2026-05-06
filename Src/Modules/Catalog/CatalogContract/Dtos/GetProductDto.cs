namespace CatalogContract.Dtos
{
    internal class GetProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<string> Photos { get; set; }
        public decimal MrpPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public string Color { get; set; }
        public int DiscountPercent { get; set; }
        public int NumRating { get; set; }
        public Guid WishListId { get; set; }
        public Guid CategoryId { get; set; }
    }
}
