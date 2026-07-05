using Module.Inventory.Contract.Dtos;
using Shared.Contract.CQRS;

namespace Module.Inventory.Features.Stocks.GetStockById;

public record GetStockByIdQuery(Guid id) : IQuery<GetStockDto>;
