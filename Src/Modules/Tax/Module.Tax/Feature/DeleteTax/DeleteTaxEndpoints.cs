using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Module.Tax.Feature.DeleteTax;

public sealed class DeleteTaxEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete("/api/tax/classes/{id}", async (Guid id, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new DeleteTaxCommand(id), ct);
            return result.Match(() => Results.NoContent(), err => Results.NotFound(err));
        })
        .WithTags("Tax")
        .WithName(nameof(DeleteTaxCommand))
        .WithSummary("Delete a tax class")
        .AllowAnonymous();
    }
}
