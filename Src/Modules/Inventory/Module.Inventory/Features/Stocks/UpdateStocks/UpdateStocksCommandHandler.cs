using Module.Inventory.Data;
using Module.Inventory.Services;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Module.Inventory.Features.Stocks.UpdateStocks;

internal class UpdateStocksCommandHandler(IStockService _stockService, InventoryDbContext _context)
    : ICommandHandler<UpdateStocksCommand>
{
    public async Task<Result> Handle(UpdateStocksCommand request, CancellationToken cancellationToken)
    {
        foreach (var dto in request.dtos)
        {

            if (dto.AdjustedQuantity == 0)
                continue;

            var result = await _stockService.UpdateStockAsync(dto.stockId, dto.AdjustedQuantity, dto.Note, cancellationToken);

            if (Error.None != result)
            {
                return Result.Failure(result);
            }


        }

        await _context.SaveChangesAsync();
        return Result.Success();
    }
}
//var stock = await _repository.GetByIdAsync(request.id, cancellationToken);
//if (stock == null)
//{
//    return Result.Failure(Error.NotFound("404", "Stock not found"));
//}

//var adjustedQuantity = request.dto.AdjustedQuantity;

//stock.UpdateQuantity(adjustedQuantity);

//var stockHistory = StockHistory.Create(stock.ProductId, stock.WarehouseId, adjustedQuantity, request.dto.Note);

//await _stockHistoryRepository.AddAsync(stockHistory, cancellationToken);

//await _repository.SaveChangesAsync();