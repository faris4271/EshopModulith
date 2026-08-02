using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Contract.ResultPattern;

namespace Eshop.Module.Basket.Feature.Querys.GetCartDetails
{
    internal class GetCartDetailsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/basket/cart-details/{id}", async (Guid id, ISender sender, CancellationToken ct) =>
            {
                var query = new GetCartDetailsQuery(id);
                var result = await sender.Send(query, ct);
                return result.Match(Results.Ok, Results.NotFound);
            }).WithTags("Cart");
        }
    }
}
