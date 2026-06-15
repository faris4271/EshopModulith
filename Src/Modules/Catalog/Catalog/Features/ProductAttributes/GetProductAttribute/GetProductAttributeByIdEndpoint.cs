using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Contract.ResultPattern;

namespace Catalog.Features.ProductAttributes.GetProductAttribute
{
    public class GetProductAttributeByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("api/product-attributes/{id:guid}", async (Guid id, ISender sender, CancellationToken ct) =>
            {
                var result = await sender.Send(new GetProductAttributeByIdQuery(id), ct);
                return result.Match(Results.Ok, error => Results.Problem(error.Error.Description));
            }).WithTags("ProductAttributes");
        }
    }
}
