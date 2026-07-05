using Module.Inventory.Data;
using Module.Inventory.Models;
using Shared.Abstraction;
using Shared.Contract.ResultPattern;

namespace Module.Inventory.Services
{
    internal class StockService(IGenericeRepository<Stock, InventoryDbContext> _repository, IGenericeRepository<StockHistory, InventoryDbContext> _stockHistoryRepository) : IStockService
    {
        public async Task<Error> UpdateStockAsync(Guid stockId, int adjustedQuantity, string note, CancellationToken cancellationToken)
        {
            var stock = await _repository.GetByIdAsync(stockId, cancellationToken);
            if (stock == null)
            {
                return Error.NotFound("404", "Stock not found");
            }

            stock.UpdateQuantity(adjustedQuantity);

            var stockHistory = StockHistory.Create(stock.ProductId, stock.WarehouseId, adjustedQuantity, note);

            await _stockHistoryRepository.AddAsync(stockHistory, cancellationToken);

            return Error.None;
        }
    }
}
