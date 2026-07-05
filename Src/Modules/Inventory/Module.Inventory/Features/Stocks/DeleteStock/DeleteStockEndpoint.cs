using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Module.Inventory.Features.Stocks.DeleteStock;

public class DeleteStockEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/stocks/{id}", async (Guid id, [FromServices] ISender sender) =>
        {
            var result = await sender.Send(new DeleteStockCommand(id));
            return result.IsSuccess ? Results.NoContent() : Results.BadRequest(result.Error);
        }).WithTags("Stock").AllowAnonymous();
    }
}
