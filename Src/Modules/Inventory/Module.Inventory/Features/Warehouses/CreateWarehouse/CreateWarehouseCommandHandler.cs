using Module.Inventory.Data;
using Module.Inventory.Models;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Module.Inventory.Features.Warehouses.CreateWarehouse;

internal class CreateWarehouseCommandHandler(IGenericeRepository<Warehouse, InventoryDbContext> _repository)
    : ICommandHandler<CreateWarehouseCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateWarehouseCommand request, CancellationToken cancellationToken)
    {
        var warehouse = Warehouse.Create(
            request.dto.Name,
            request.dto.VendorId,
            request.dto.Street,
            request.dto.City,
            request.dto.State,
            request.dto.ZipCode,
            request.dto.Country,
            request.dto.Phone,
            request.dto.PostalCode
        );

        await _repository.AddAsync(warehouse, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return Result.Success(warehouse.Id);
    }
}
