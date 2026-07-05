using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Module.Inventory.Features.Warehouses.GetWarehouseById;

public class GetWarehouseByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/warehouses/{id}", async (Guid id, [FromServices] ISender sender) =>
        {
            var result = await sender.Send(new GetWarehouseByIdQuery(id));
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Error);
        }).WithTags("Warehouse").AllowAnonymous();
    }
}
