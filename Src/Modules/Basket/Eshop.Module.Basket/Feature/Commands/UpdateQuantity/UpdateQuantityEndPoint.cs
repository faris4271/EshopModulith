using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Eshop.Module.Basket.Feature.Commands.UpdateQuantity
{
    internal class UpdateQuantityEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/api/basket/items/quantity", async ([FromBody] UpdateQuantityCommand command, [FromServices] ISender sender) =>
            {
                var result = await sender.Send(command);
                return result.Match(
                    Results.NoContent,
                    Results.BadRequest
                );
            }).WithTags("Basket");
        }
    }
}
