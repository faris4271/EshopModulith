namespace Module.Inventory.Contract.Dtos;

public record GetWarehouseDto(
    Guid Id,
    string Name,
    Guid? VendorId,
    string Street,
    string City,
    string State,
    string ZipCode,
    string Country,
    string Phone,
    string PostalCode
);
