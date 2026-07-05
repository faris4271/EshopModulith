namespace Module.Inventory.Contract.Dtos;

public record UpdateWarehouseDto(
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
