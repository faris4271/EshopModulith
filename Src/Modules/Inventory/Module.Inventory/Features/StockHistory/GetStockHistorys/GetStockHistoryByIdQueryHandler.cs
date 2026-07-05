
using Microsoft.EntityFrameworkCore;
using Module.Inventory.Contract.Dtos;
using Module.Inventory.Data;
using Module.Inventory.Features.StockHistorys.GetStockHistory;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Module.Inventory.Features.StockHistorys.GetStockHistorys
{
    internal class GetStockHistoryByIdQueryHandler(IGenericeRepository<Models.StockHistory, InventoryDbContext> _repository) : IQueryHandler<GetStockHistoryByIdQuery, List<StockHistoryDto>>
    {
        public async Task<Result<List<StockHistoryDto>>> Handle(GetStockHistoryByIdQuery request, CancellationToken cancellationToken)
        {
            var query = await _repository.Query();

            var stockHistory = await query.Where(x => x.ProductId == request.product && x.WarehouseId == request.warehouseId).ToListAsync();

            if (stockHistory == null)
                return Result.Failure<List<StockHistoryDto>>(Error.NullValue);

            var result = stockHistory.Select(x => new StockHistoryDto
            (
                x.Id,
                x.ProductId,
                x.WarehouseId,
                (int)x.AdjustedQuantity,
                x.Note,
                x.CreatedOn,
                x.CreatedById
            )).ToList();

            return Result.Success(result);
        }
    }
}
