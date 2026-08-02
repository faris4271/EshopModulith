using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Module.Tax.Feature.UpdateTax;

public sealed class UpdateTaxEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut("/api/tax/classes/{id}", async (Guid id, UpdateTaxCommand command, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(command with { Id = id }, ct);
            return result.Match(() => Results.NoContent(), err => Results.BadRequest(err));
        })
        .WithTags("Tax")
        .WithName(nameof(UpdateTaxCommand))
        .WithSummary("Update a tax class")
        .AllowAnonymous();
    }
}
