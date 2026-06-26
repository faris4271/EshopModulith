namespace CatalogContract.Dtos
{
    public record UpdateProductTempletDto : CreatProductTempletDto
    {
        public Guid Id { get; set; }
    }
}
