using Module.Inventory.Contract.Dtos;
using Shared.Contract.CQRS;

namespace Module.Inventory.Features.Stocks.UpdateStocks;

public record UpdateStocksCommand(List<UpdateStockDto> dtos) : ICommand;
