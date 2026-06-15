using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Contract.ResultPattern;

namespace Catalog.Features.ProductAttributeGroups.GetProductAttributeGroups;

public sealed class GetProductAttributeGroupsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/product-attribut-group", async (ISender sender, CancellationToken ct) =>
            {
                var result = await sender.Send(new GetProductAttributeGroupsQuery(), ct);

                return result.Match(Results.Ok, Results.NotFound);
            }).
            WithTags("ProductAttributeGroups")
        .WithName(nameof(GetProductAttributeGroupsQuery))
        .WithSummary("Get all product attribute groups");
    }
}
