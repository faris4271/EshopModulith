using Module.Inventory.Data;
using Module.Inventory.Models;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Module.Inventory.Features.Warehouses.DeleteWarehouse;

internal class DeleteWarehouseCommandHandler(IGenericeRepository<Warehouse, InventoryDbContext> _repository)
    : ICommandHandler<DeleteWarehouseCommand>
{
    public async Task<Result> Handle(DeleteWarehouseCommand request, CancellationToken cancellationToken)
    {
        var warehouse = await _repository.GetByIdAsync(request.id, cancellationToken);
        if (warehouse == null)
        {
            return Result.Failure(Error.NotFound("404", "Warehouse not found"));
        }

        _repository.Delete(warehouse);
        await _repository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
