using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Contract.ResultPattern;

namespace Catalog.Features.ProductTemplet.GetProductTemplets
{
    internal class GetProductTempletsEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/product-templet", async (ISender sender) =>
            {
                var result = await sender.Send(new GetProductTempletsQuery());

                return result.Match(Results.Ok, Results.BadRequest);

            }).WithTags("Product-Templet");
        }
    }
}
