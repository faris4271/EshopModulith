using CatalogContract.Features.GetStockProducts;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Module.Inventory.Contract.Dtos;
using Module.Inventory.Data;
using Module.Inventory.Models;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using Shared.Web.SmartTable;

namespace Module.Inventory.Features.Stocks.GetStocks;

internal class GetStocksQueryHandler(IGenericeRepository<Stock, InventoryDbContext> _repository, ISender sender)
    : IQueryHandler<GetStocksQuery, SmartTableResult<GetStockDto>>
{
    public async Task<Result<SmartTableResult<GetStockDto>>> Handle(GetStocksQuery request, CancellationToken cancellationToken)
    {
        var query = await _repository.GetAllAsQuerable();

        IQueryable<Stock> stocksQuery = query
            .AsNoTracking()
            .Include(s => s.Warehouse).Where(x => x.WarehouseId == request.WarehouseId);

        var param = request.SmartTableParam;
        if (param.Search.PredicateObject != null)
        {
            dynamic search = param.Search.PredicateObject;
            if (search.Sku != null)
            {
                string sku = search.Sku;
                stocksQuery = stocksQuery.Where(x => x.sku.Contains(sku));
            }
        }

        var pagedResult = stocksQuery.ToSmartTableResult(request.SmartTableParam, stock => new GetStockDto
        {
            Id = stock.Id,
            ProductId = stock.ProductId,
            ProductName = string.Empty,
            WarehouseId = stock.WarehouseId,
            Quantity = stock.Quantity,
            Sku = stock.sku,
            ReservedQuantity = stock.ReservedQuantity,
            WarehouseName = stock.Warehouse.Name.name
        });

        if (pagedResult.Items == null || !pagedResult.Items.Any())
            return Result.Success(pagedResult);

        var currentProductIds = pagedResult.Items
            .Select(x => x.ProductId)
            .Distinct()
            .ToList();

        var productResult = await sender.Send(new GetStockProductsQuery(currentProductIds), cancellationToken);

        if (!productResult.IsSuccess)
            return Result.Failure<SmartTableResult<GetStockDto>>(productResult.Error);

        var productDict = productResult.Value
            .Where(p => p != null)
            .ToDictionary(p => p.productId, p => p.productName);

        foreach (var stockDto in pagedResult.Items)
        {
            if (productDict.TryGetValue(stockDto.ProductId, out var name))
            {
                stockDto.ProductName = name ?? string.Empty;
            }
        }

        return Result.Success(pagedResult);
    }
}