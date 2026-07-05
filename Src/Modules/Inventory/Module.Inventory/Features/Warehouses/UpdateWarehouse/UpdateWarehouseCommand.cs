using Module.Inventory.Contract.Dtos;
using Shared.Contract.CQRS;

namespace Module.Inventory.Features.Warehouses.UpdateWarehouse;

public record UpdateWarehouseCommand(Guid id, UpdateWarehouseDto dto) : ICommand;
