using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Module.Inventory.Contract.Dtos;

namespace Module.Inventory.Features.Warehouses.CreateWarehouse;

public class CreateWarehouseEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/warehouses", async ([FromBody] CreateWarehouseDto dto, [FromServices] ISender sender) =>
        {
            var result = await sender.Send(new CreateWarehouseCommand(dto));
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
        }).WithTags("Warehouse").AllowAnonymous();
    }
}
