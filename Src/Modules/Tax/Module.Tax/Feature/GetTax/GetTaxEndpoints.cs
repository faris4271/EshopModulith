using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Contract.ResultPattern;

namespace Module.Tax.Feature.GetTax;

public sealed class GetTaxEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/tax/classes/{id}", async (Guid id, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetTaxQuery(id), ct);
            return result.Match(Results.Ok, Results.NotFound);
        })
        .WithTags("Tax")
        .WithName(nameof(GetTaxQuery))
        .WithSummary("Get a tax class by ID")
        .AllowAnonymous();
    }
}
