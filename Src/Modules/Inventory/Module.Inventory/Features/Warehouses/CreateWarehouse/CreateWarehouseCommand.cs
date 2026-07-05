using Module.Inventory.Contract.Dtos;
using Shared.Contract.CQRS;

namespace Module.Inventory.Features.Warehouses.CreateWarehouse;

public record CreateWarehouseCommand(CreateWarehouseDto dto) : ICommand<Guid>;
