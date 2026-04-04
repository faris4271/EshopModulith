namespace Catalog.Products.Dtos
{
    public class ProductLinkDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsPublished { get; set; }
    }
}