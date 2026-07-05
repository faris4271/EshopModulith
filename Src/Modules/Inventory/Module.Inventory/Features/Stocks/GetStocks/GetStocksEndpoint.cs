using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Shared.Contract.ResultPattern;
using Shared.Web.SmartTable;

namespace Module.Inventory.Features.Stocks.GetStocks;

public class GetStocksEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/inventory/stocks/grid/{warehouseId:guid}", async ([FromServices] ISender sender, Guid warehouseId, HttpContext context) =>
        {
            using var reader = new StreamReader(context.Request.Body);
            var bodyString = await reader.ReadToEndAsync();
            var smartTableParam = Newtonsoft.Json.JsonConvert.DeserializeObject<SmartTableParam>(bodyString);
            var result = await sender.Send(new GetStocksQuery(warehouseId, smartTableParam));
            return result.Match(Results.Ok, Results.NotFound);
        }).WithTags("Stock");
    }
}
