using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Identity.Contract.Feature.Vendors;

namespace IdentityModule.Feature.Vendors.CreatVendors
{
    internal class CreatVendorEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/vendors", async (CreatVendorCommand command, ISender sender) =>
            {
                var result = await sender.Send(command);
                return result.IsSuccess ? Results.NoContent() : Results.BadRequest();
            }).WithTags("Vendors");
        }
    }
}
