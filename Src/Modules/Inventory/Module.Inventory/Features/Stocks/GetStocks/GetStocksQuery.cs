using Module.Inventory.Contract.Dtos;
using Shared.Contract.CQRS;
using Shared.Web.SmartTable;

namespace Module.Inventory.Features.Stocks.GetStocks;

public record GetStocksQuery(Guid WarehouseId, SmartTableParam SmartTableParam) : IQuery<SmartTableResult<GetStockDto>>;
