using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Module.Tax.Feature.DeleteTaxRate;

public sealed class DeleteTaxRateEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete("/api/tax/rates/{id}", async (Guid id, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new DeleteTaxRateCommand(id), ct);
            return result.Match(() => Results.NoContent(), err => Results.NotFound(err));
        })
        .WithTags("TaxRate")
        .WithName(nameof(DeleteTaxRateCommand))
        .WithSummary("Delete a tax rate")
        .AllowAnonymous();
    }
}
