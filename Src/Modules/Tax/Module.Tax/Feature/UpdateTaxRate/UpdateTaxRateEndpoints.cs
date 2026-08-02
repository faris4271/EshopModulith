using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Module.Tax.Feature.UpdateTaxRate;

public sealed class UpdateTaxRateEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut("/api/tax/rates/{id}", async (Guid id, UpdateTaxRateCommand command, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(command with { Id = id }, ct);
            return result.Match(() => Results.NoContent(), err => Results.BadRequest(err));
        })
        .WithTags("TaxRate")
        .WithName(nameof(UpdateTaxRateCommand))
        .WithSummary("Update a tax rate");

    }
}
