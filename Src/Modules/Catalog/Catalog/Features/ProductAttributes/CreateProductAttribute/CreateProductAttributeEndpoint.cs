using Carter;
using CatalogContract.Dtos;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Features.ProductAttributes.CreateProductAttribute
{
    public class CreateProductAttributeEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("api/product-attributes", async (CreateProductAttributeDto dto, ISender sender, CancellationToken ct) =>
            {
                var command = new CreateProductAttributeCommand(dto);
                var result = await sender.Send(command, ct);

                return result.Match(
                  Results.Created, Results.BadRequest
                );
            }).WithTags("ProductAttributes");
        }
    }
}
