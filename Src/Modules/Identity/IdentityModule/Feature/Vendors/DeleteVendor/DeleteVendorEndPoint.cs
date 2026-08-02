using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Identity.Contract.Feature.Vendors.DeleteVendor;
using Shared.Contract.ResultPattern;

namespace IdentityModule.Feature.Vendors.DeleteVendor;

internal class DeleteVendorEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/vendors/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new DeleteVendorCommand(id));
            return result.Match(
                _ => Results.NoContent(),
                errors => Results.NotFound());
        }).WithTags("Vendors")
        .WithName(nameof(DeleteVendorCommand))
        .WithSummary("Delete a vendor")
        .AllowAnonymous();
    }
}