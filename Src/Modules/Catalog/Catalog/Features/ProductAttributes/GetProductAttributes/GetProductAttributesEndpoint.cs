using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Contract.ResultPattern;

namespace Catalog.Features.ProductAttributes.GetProductAttributes
{
    public class GetProductAttributesEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("api/product-attributes", async (ISender sender, CancellationToken ct) =>
            {
                var result = await sender.Send(new GetProductAttributesQuery(), ct);
                return result.Match(Results.Ok, error => Results.Problem(error.Description));
            }).WithTags("ProductAttributes");
        }
    }
}
