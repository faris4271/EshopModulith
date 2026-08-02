using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Identity.Contract.Feature.Vendors.UpdateVendor;

namespace IdentityModule.Feature.Vendors.UpdateVendor;

internal class UpdateVendorEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/vendors/{id}", async (Guid id, UpdateVendorCommand command, ISender sender) =>
        {
            var result = await sender.Send(command with { Id = id });
            return result.IsSuccess ? Results.NoContent() : Results.NotFound();
        }).WithTags("Vendors")
        .WithName(nameof(UpdateVendorCommand))
        .WithSummary("Update a vendor")
        .AllowAnonymous();
    }
}