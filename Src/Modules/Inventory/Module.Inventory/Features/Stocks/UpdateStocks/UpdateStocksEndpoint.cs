using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Module.Inventory.Contract.Dtos;

namespace Module.Inventory.Features.Stocks.UpdateStocks;

public class UpdateStocksEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/inventory/stocks", async ([FromBody] List<UpdateStockDto> dtos, [FromServices] ISender sender) =>
        {
            var result = await sender.Send(new UpdateStocksCommand(dtos));
            return result.IsSuccess ? Results.NoContent() : Results.BadRequest(result.Error);
        }).WithTags("Stock");
    }
}
