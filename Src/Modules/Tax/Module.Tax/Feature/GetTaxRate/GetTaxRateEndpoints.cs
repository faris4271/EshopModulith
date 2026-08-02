using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Contract.ResultPattern;

namespace Module.Tax.Feature.GetTaxRate;

public sealed class GetTaxRateEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/tax/rates/{id}", async (Guid id, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetTaxRateQuery(id), ct);
            return result.Match(Results.Ok, Results.NotFound);
        })
        .WithTags("TaxRate")
        .WithName(nameof(GetTaxRateQuery))
        .WithSummary("Get a tax rate by ID")
        .AllowAnonymous();
    }
}
