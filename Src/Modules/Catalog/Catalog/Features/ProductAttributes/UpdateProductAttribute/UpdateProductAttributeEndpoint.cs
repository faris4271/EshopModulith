using Carter;
using CatalogContract.Dtos;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Catalog.Features.ProductAttributes.UpdateProductAttribute
{
    public class UpdateProductAttributeEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("api/product-attributes/{id:guid}", async (Guid id, UpdateProductAttributeDto dto, ISender sender, CancellationToken ct) =>
            {
                var command = new UpdateProductAttributeCommand(id, dto);
                var result = await sender.Send(command, ct);

                return result.Match(
                 Results.Created,Results.BadRequest

                );
            }).WithTags("ProductAttributes");
        }
    }
}
