using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Module.Inventory.Features.StockHistorys.GetStockHistory;

namespace Module.Inventory.Features.StockHistory.GetStockHistorys
{
    internal class GetStockHistoryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("api/inventory/stocks/{product:guid}/{warehouseId:guid}", async (Guid product, Guid warehouseId, [FromServices] ISender sender) =>
            {
                var result = await sender.Send(new GetStockHistoryByIdQuery(product, warehouseId));
                return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Error);
            }).WithTags("StockHistory");
        }
    }
}
