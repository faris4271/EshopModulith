using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Catalog.Features.ProductAttributeGroups.DeleteProductAttributeGroup;

public sealed class DeleteProductAttributeGroupEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete("api/attribute-group/{id:guid}", async (Guid id, ISender sender, CancellationToken ct) =>
            {
                var result = await sender.Send(new DeleteProductAttributeGroupCommand(id), ct);
                return result.Match(Results.NoContent, Results.BadRequest);
            }).
            WithTags("ProductAttributeGroups")
        .WithName(nameof(DeleteProductAttributeGroupCommand))
        .WithSummary("Delete a product attribute group");
    }
}
