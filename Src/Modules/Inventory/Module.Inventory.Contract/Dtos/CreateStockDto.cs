namespace Module.Inventory.Contract.Dtos;

public record CreateStockDto(
    Guid ProductId,
    Guid WarehouseId,
    int Quantity,
    int ReservedQuantity
);
