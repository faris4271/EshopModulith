using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Eshop.Module.Pricing.Feature.Commands.CreatCartRule
{
    internal class CreatCartRuleEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/p/cart-rules", async ([FromBody] CreatCartRuleCommand command, [FromServices] ISender sender) =>
            {

                var result = await sender.Send(command);

                return result.Match(
                    Results.Created,
                    Results.BadRequest
                );
            }).WithTags("Cart");
        }
    }
}
