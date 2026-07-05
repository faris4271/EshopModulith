using Mapster;
using Module.Inventory.Contract.Dtos;
using Module.Inventory.Data;
using Module.Inventory.Models;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Module.Inventory.Features.Warehouses.GetWarehouseById;

internal class GetWarehouseByIdQueryHandler(IGenericeRepository<Warehouse, InventoryDbContext> _repository)
    : IQueryHandler<GetWarehouseByIdQuery, GetWarehouseDto>
{
    public async Task<Result<GetWarehouseDto>> Handle(GetWarehouseByIdQuery request, CancellationToken cancellationToken)
    {
        var warehouse = await _repository.GetByIdAsync(request.id, cancellationToken);
        if (warehouse == null)
        {
            return Result.Failure<GetWarehouseDto>(Error.NotFound("404", "Warehouse not found"));
        }

        return Result.Success(warehouse.Adapt<GetWarehouseDto>());
    }
}
