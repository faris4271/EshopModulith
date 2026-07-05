using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Module.Inventory.Contract.Dtos;

namespace Module.Inventory.Features.Warehouses.UpdateWarehouse;

public class UpdateWarehouseEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/warehouses/{id}", async (Guid id, [FromBody] UpdateWarehouseDto dto, [FromServices] ISender sender) =>
        {
            var result = await sender.Send(new UpdateWarehouseCommand(id, dto));
            return result.IsSuccess ? Results.NoContent() : Results.BadRequest(result.Error);
        }).WithTags("Warehouse").AllowAnonymous();
    }
}
