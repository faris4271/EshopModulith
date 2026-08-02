namespace CatalogContract.Dtos
{
    public record ProductDto : CreatProductDto
    {
        public Guid Id { get; set; }
    }

}
