using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Contract.ResultPattern;

namespace Eshop.Module.Pricing.Feature.Querys.GetCartRuleById
{
    internal class GetCartRuleByIdEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/basket/cart-rules/{id}", async (Guid id, ISender sender, CancellationToken ct) =>
            {
                var result = await sender.Send(new GetCartRuleByIdQuery(id), ct);
                return result.Match(Results.Ok, Results.NotFound);
            }).WithTags("Cart");
        }
    }
}
