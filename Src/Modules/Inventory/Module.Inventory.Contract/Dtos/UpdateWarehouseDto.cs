namespace Module.Inventory.Contract.Dtos;

public record UpdateWarehouseDto(
    Guid Id,
    string Name,
    Guid? VendorId,
   Guid AddressId
);
