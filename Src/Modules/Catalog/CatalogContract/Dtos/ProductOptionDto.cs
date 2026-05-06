namespace CatalogContract.Dtos
{
    public class ProductOptionDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string DisplayType { get; set; }

        public IList<ProductOptionValueDto> Values { get; set; } = new List<ProductOptionValueDto>();
    }
}