using Module.Inventory.Contract.Dtos;
using Shared.Contract.CQRS;

namespace Module.Inventory.Features.Warehouses.GetWarehouses;

public record GetWarehousesQuery : IQuery<List<GetWarehouseDto>>;
