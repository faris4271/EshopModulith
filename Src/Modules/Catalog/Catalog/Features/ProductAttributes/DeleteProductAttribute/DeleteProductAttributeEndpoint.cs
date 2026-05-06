using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using Shared.Contract.ResultPattern;

namespace Catalog.Features.ProductAttributes.DeleteProductAttribute
{
    public class DeleteProductAttributeEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("api/product-attributes/{id:guid}", async (Guid id, ISender sender, CancellationToken ct) =>
            {
                var command = new DeleteProductAttributeCommand(id);
                var result = await sender.Send(command, ct);

                result.Match(Results.NoContent, error => Results.Problem(error.Description));

               
            }).WithTags("ProductAttributes");
        }
    }
}
