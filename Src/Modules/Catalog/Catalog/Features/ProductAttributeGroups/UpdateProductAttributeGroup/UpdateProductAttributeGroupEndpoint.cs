using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Catalog.Features.ProductAttributeGroups.UpdateProductAttributeGroup;

public sealed class UpdateProductAttributeGroupEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut("api/attribute-group/{id:guid}", async (Guid id, UpdateProductAttributeGroupCommand cmd, ISender sender, CancellationToken ct) =>
            {
                var updateCmd = cmd with { Id = id };
                var result = await sender.Send(updateCmd, ct);
                return result.Match(Results.NoContent, Results.BadRequest);
            }).
            WithTags("ProductAttributeGroups")
        .WithName(nameof(UpdateProductAttributeGroupCommand))
        .WithSummary("Update a product attribute group");
    }
}
