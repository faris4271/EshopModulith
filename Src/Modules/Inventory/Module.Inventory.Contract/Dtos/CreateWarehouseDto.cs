namespace Module.Inventory.Contract.Dtos;

public record CreateWarehouseDto(
    string Name,
    Guid? VendorId,
   Guid AddressId
);
