using Shared.Contract.CQRS;

namespace Module.Inventory.Features.Warehouses.DeleteWarehouse;

public record DeleteWarehouseCommand(Guid id) : ICommand;
