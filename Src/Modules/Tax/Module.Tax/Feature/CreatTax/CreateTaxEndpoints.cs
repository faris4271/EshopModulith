using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Contract.ResultPattern;

namespace Module.Tax.Feature.CreatTax;

public sealed class CreateTaxEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/tax/classes", async (CreateTaxCommand command, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(command, ct);
            return result.Match(
                id => Results.Created($"/api/v1/tax/classes/{id}", id),
                err => Results.BadRequest(err));
        })
        .WithTags("Tax")
        .WithName(nameof(CreateTaxCommand))
        .WithSummary("Create a new tax class")
        .AllowAnonymous();
    }
}
