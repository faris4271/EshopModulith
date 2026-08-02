using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Eshop.Module.Pricing.Feature.Commands.UpdateCartRule
{
    internal class UpdateCartRuleEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/api/basket/cart-rules/{id}", async (Guid id, [FromBody] UpdateCartRuleCommand command, [FromServices] ISender sender) =>
            {
                var result = await sender.Send(command with { Id = id });

                return result.Match(
                    Results.NoContent,
                    Results.BadRequest
                );
            })
            .WithTags("Cart")
            .WithName(nameof(UpdateCartRuleCommand))
            .WithSummary("Update a cart rule")
            .AllowAnonymous();
        }
    }
}
