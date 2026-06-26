using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Contract.ResultPattern;

namespace Catalog.Features.ProductAttributeGroups.GetProductAttributeGroupById;

public sealed class GetProductAttributeGroupByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("api/attribute-group/{id:guid}", async (Guid id, ISender sender, CancellationToken ct) =>
            {
                var result = await sender.Send(new GetProductAttributeGroupByIdQuery(id), ct);
                return result.Match(Results.Ok, Results.NotFound);
            }).
            WithTags("ProductAttributeGroups")
        .WithName(nameof(GetProductAttributeGroupByIdQuery))
        .WithSummary("Get a product attribute group by ID");
    }
}
