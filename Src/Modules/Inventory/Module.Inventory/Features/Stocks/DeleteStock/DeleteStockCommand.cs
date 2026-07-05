using Shared.Contract.CQRS;

namespace Module.Inventory.Features.Stocks.DeleteStock;

public record DeleteStockCommand(Guid id) : ICommand;
