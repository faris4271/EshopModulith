namespace Catalog.Products.Dtos
{
    public class ProductOptionCombinationDto
    {
        public Guid OptionId { get; set; }

        public string OptionName { get; set; }

        public string Value { get; set; }

        public int SortIndex { get; set; }
    }
}