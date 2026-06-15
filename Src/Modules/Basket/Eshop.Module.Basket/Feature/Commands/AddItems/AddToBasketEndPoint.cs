using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Shared.Contract.ResultPattern;

namespace Eshop.Module.Basket.Feature.Commands.AddItems
{
    partial class AddToBasketCommandRequest
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
    internal class AddToBasketEndPoint : ICarterModule
    {

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/basket/items", async ([FromBody] AddToBasketCommandRequest request, [FromServices] ISender sender) =>
            {
                var command = new AddToBasketCommand(request.ProductId, request.Quantity);
                var result = await sender.Send(command);

                return result.Match(
                    Results.Ok,
                    Results.BadRequest
                );
            }).WithTags("Basket");


        }
    }
}
