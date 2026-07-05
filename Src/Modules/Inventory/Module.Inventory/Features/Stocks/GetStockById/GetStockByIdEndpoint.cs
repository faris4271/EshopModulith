using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Module.Inventory.Features.Stocks.GetStockById;

public class GetStockByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/stocks/{id}", async (Guid id, [FromServices] ISender sender) =>
        {
            var result = await sender.Send(new GetStockByIdQuery(id));
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Error);
        }).WithTags("Stock").AllowAnonymous();
    }
}
