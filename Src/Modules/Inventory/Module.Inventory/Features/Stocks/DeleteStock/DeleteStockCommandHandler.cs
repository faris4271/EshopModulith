using Module.Inventory.Data;
using Module.Inventory.Models;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Module.Inventory.Features.Stocks.DeleteStock;

internal class DeleteStockCommandHandler(IGenericeRepository<Stock, InventoryDbContext> _repository)
    : ICommandHandler<DeleteStockCommand>
{
    public async Task<Result> Handle(DeleteStockCommand request, CancellationToken cancellationToken)
    {
        var stock = await _repository.GetByIdAsync(request.id, cancellationToken);
        if (stock == null)
        {
            return Result.Failure(Error.NotFound("404", "Stock not found"));
        }

        _repository.Delete(stock);
        await _repository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
