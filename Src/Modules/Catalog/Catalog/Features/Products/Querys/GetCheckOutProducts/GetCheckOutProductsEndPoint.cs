using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; // For [FromServices]
using Microsoft.AspNetCore.Routing;
using Shared.Contract.ResultPattern;
using CatalogContract.Features.GetCheckOutProducts;

namespace Catalog.Features.Products.Querys.GetCheckOutProducts
{
    internal class GetCheckOutProductsEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("api/products/checkout-items", async (GetCheckOutProductsQuery request, [FromServices] ISender send) =>
            {
                var response = await send.Send(request);

                return response.Match(Results.Ok, Results.BadRequest);

            }).WithTags("Products")
               .WithName(nameof(GetCheckOutProductsQuery))
               .WithSummary("Get checkout items for specified product IDs")
               .AllowAnonymous();
        }
    }
}