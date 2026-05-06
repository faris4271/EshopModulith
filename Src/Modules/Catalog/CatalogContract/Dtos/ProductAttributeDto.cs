namespace CatalogContract.Dtos
{
    public class ProductAttributeDto
    {
        public Guid Id { get; set; }

        public Guid AttributeValueId { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public string GroupName { get; set; }
    }
}