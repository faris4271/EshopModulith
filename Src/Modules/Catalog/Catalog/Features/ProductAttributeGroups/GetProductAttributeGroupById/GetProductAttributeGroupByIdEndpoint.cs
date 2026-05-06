using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Catalog.Features.ProductAttributeGroups.GetProductAttributeGroupById;

public sealed class GetProductAttributeGroupByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/{id:guid}", async (Guid id, ISender sender, CancellationToken ct) =>
            {
                var result = await sender.Send(new GetProductAttributeGroupByIdQuery(id), ct);
                return result == null ? Results.NotFound() : Results.Ok(result);
            }).
            WithTags("ProductAttributeGroups")
        .WithName(nameof(GetProductAttributeGroupByIdQuery))
        .WithSummary("Get a product attribute group by ID");
    }
}
