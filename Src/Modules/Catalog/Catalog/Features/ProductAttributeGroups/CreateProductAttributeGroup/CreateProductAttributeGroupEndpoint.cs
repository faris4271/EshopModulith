using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Catalog.Features.ProductAttributeGroups.CreateProductAttributeGroup;

public sealed class CreateProductAttributeGroupEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/attribute-group", async (CreateProductAttributeGroupCommand cmd, ISender sender, CancellationToken ct) =>
            {
                var result = await sender.Send(cmd, ct);
                return result.Match(Results.Created, Results.BadRequest);
            }).
            WithTags("ProductAttributeGroups")
        .WithName(nameof(CreateProductAttributeGroupCommand))
        .WithSummary("Create a new product attribute group");
    }
}
