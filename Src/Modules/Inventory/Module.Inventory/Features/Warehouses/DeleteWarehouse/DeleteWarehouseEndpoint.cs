using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Module.Inventory.Features.Warehouses.DeleteWarehouse;

public class DeleteWarehouseEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/warehouses/{id}", async (Guid id, [FromServices] ISender sender) =>
        {
            var result = await sender.Send(new DeleteWarehouseCommand(id));
            return result.IsSuccess ? Results.NoContent() : Results.BadRequest(result.Error);
        }).WithTags("Warehouse").AllowAnonymous();
    }
}
