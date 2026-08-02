using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Contract.ResultPattern;

namespace Module.Tax.Feature.CreateTaxRate;

public sealed class CreateTaxRateEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/tax/rates", async (CreateTaxRateCommand command, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(command, ct);
            return result.Match(
                id => Results.Created($"/api/v1/tax/rates/{id}", id),
                err => Results.BadRequest(err));
        })
        .WithTags("TaxRate")
        .WithName(nameof(CreateTaxRateCommand))
        .WithSummary("Create a new tax rate")
        .AllowAnonymous();
    }
}
