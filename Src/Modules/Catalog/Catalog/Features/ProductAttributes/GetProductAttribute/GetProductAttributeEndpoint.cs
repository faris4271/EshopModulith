using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;
using Shared.Contract.ResultPattern;
using Microsoft.AspNetCore.Http;

namespace Catalog.Features.ProductAttributes.GetProductAttribute
{
    public class GetProductAttributeEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("api/product-attributes/{id:guid}", async (Guid id, ISender sender, CancellationToken ct) =>
            {
                var result = await sender.Send(new GetProductAttributeQuery(id), ct);
               return result.Match(Results.Ok, error => Results.Problem(error.Error.Description));
            }).WithTags("ProductAttributes");
        }
    }
}
