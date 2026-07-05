using Microsoft.EntityFrameworkCore;
using Module.Inventory.Contract.Dtos;
using Module.Inventory.Data;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Module.Inventory.Features.Stocks.GetStockById;

internal class GetStockByIdQueryHandler(InventoryDbContext _context)
    : IQueryHandler<GetStockByIdQuery, GetStockDto>
{
    public async Task<Result<GetStockDto>> Handle(GetStockByIdQuery request, CancellationToken cancellationToken)
    {
        var stock = await _context.Stocks
            .Include(s => s.Warehouse)
            .FirstOrDefaultAsync(s => s.Id == request.id, cancellationToken);

        if (stock == null)
        {
            return Result.Failure<GetStockDto>(Error.NotFound("404", "Stock not found"));
        }

        return Result.Success(new GetStockDto
        {
            Id = stock.Id,
            ProductId = stock.ProductId,
            WarehouseId = stock.WarehouseId,
            Quantity = stock.Quantity,
            ReservedQuantity = stock.ReservedQuantity,
            WarehouseName = stock.Warehouse?.Name.name
        });

    }
}
