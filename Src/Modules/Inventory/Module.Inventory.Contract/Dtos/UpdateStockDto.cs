namespace Module.Inventory.Contract.Dtos;

public record UpdateStockDto(


Guid stockId,

Guid ProductId,

 Guid WarehouseId,

 int AdjustedQuantity,

 string Note
);
