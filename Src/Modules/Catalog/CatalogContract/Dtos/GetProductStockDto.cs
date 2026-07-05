namespace CatalogContract.Dtos
{
    public record GetProductStockDto(
        Guid productId,
        string? productName,
        string Sku
    );

}
