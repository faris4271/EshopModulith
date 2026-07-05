namespace Module.Inventory.Contract.Dtos
{
    public record StockHistoryDto(
        Guid Id, Guid ProductId,
        Guid WarehouseId, int AdjustedQuantity,
        string Note, DateTimeOffset CreatedOn, string CreatedBy);


}
