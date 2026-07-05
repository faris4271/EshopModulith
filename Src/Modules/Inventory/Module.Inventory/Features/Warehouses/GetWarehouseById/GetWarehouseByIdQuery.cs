using Module.Inventory.Contract.Dtos;
using Shared.Contract.CQRS;

namespace Module.Inventory.Features.Warehouses.GetWarehouseById;

public record GetWarehouseByIdQuery(Guid id) : IQuery<GetWarehouseDto>;
