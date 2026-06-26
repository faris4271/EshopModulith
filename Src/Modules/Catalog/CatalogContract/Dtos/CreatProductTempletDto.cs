namespace CatalogContract.Dtos
{
    public record CreatProductTempletDto
    {
        public string Name { get; init; } = string.Empty;

        public List<Guid> AttributeIds { get; init; } = new();
    }
}
