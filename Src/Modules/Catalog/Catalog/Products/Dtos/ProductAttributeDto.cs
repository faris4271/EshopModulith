namespace Catalog.Products.Dtos
{
    public class ProductAttributeDto
    {
        public Guid Id { get; set; }

        public long AttributeValueId { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public string GroupName { get; set; }
    }
}