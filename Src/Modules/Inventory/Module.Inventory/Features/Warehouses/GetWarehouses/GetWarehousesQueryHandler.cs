using Module.Inventory.Contract.Dtos;
using Module.Inventory.Data;
using Module.Inventory.Models;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Module.Inventory.Features.Warehouses.GetWarehouses;

internal class GetWarehousesQueryHandler(IGenericeRepository<Warehouse, InventoryDbContext> _repository)
    : IQueryHandler<GetWarehousesQuery, List<GetWarehouseDto>>
{
    public async Task<Result<List<GetWarehouseDto>>> Handle(GetWarehousesQuery request, CancellationToken cancellationToken)
    {
        var warehouses = await _repository.GetAllAsync(cancellationToken);
        var dtos = warehouses.Select(x => new GetWarehouseDto
        (
            x.Id,
            x.Name.name,
            x.VendorId,
            x.Address.Street,
            x.Address.City,
            x.Address.State,
            x.Address.ZipCode,
            x.Address.Country,
            x.Address.Phone,
            x.Address.PostalCode
        )).ToList();
        return Result.Success(dtos);
    }
}
