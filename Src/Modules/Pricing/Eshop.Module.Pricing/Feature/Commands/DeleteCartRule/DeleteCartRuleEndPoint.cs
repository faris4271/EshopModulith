using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Eshop.Module.Pricing.Feature.Commands.DeleteCartRule
{
    internal class DeleteCartRuleEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/api/basket/cart-rules/{id}", async (Guid id, [FromServices] ISender sender) =>
            {
                var result = await sender.Send(new DeleteCartRuleCommand(id));

                return result.Match(
                    Results.NoContent,
                    Results.NotFound
                );
            })
            .WithTags("Cart")
            .WithName(nameof(DeleteCartRuleCommand))
            .WithSummary("Delete a cart rule")
            .AllowAnonymous();
        }
    }
}
