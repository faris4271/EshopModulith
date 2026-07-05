namespace Module.Inventory.Contract.Dtos;

public class GetStockDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public Guid WarehouseId { get; set; }
    public int Quantity { get; set; }

    public string Sku { get; set; }
    public int ReservedQuantity { get; set; }
    public string? WarehouseName { get; set; }
}
