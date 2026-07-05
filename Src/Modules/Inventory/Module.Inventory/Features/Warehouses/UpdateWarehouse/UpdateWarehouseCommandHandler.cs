using Module.Inventory.Data;
using Module.Inventory.Models;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Module.Inventory.Features.Warehouses.UpdateWarehouse;

internal class UpdateWarehouseCommandHandler(IGenericeRepository<Warehouse, InventoryDbContext> _repository)
    : ICommandHandler<UpdateWarehouseCommand>
{
    public async Task<Result> Handle(UpdateWarehouseCommand request, CancellationToken cancellationToken)
    {
        var warehouse = await _repository.GetByIdAsync(request.id, cancellationToken);
        if (warehouse == null)
        {
            return Result.Failure(Error.NotFound("404", "Warehouse not found"));
        }

        warehouse.Update(
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

        await _repository.UpdateAsync(warehouse, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
