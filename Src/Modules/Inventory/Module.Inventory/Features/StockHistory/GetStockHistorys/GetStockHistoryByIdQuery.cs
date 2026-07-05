using Module.Inventory.Contract.Dtos;
using Shared.Contract.CQRS;

namespace Module.Inventory.Features.StockHistorys.GetStockHistory
{
    public record GetStockHistoryByIdQuery(Guid product, Guid warehouseId) : IQuery<List<StockHistoryDto>>
    {
    }
}
