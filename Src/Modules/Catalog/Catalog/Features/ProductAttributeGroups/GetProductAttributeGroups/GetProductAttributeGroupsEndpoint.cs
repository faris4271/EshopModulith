using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Catalog.Features.ProductAttributeGroups.GetProductAttributeGroups;

public sealed class GetProductAttributeGroupsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/", async (ISender sender, CancellationToken ct) =>
            {
                return await sender.Send(new GetProductAttributeGroupsQuery(), ct);
            }).
            WithTags("ProductAttributeGroups")
        .WithName(nameof(GetProductAttributeGroupsQuery))
        .WithSummary("Get all product attribute groups");
    }
}
