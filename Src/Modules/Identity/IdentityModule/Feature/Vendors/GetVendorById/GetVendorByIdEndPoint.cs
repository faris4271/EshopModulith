using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Identity.Contract.Feature.Vendors;
using Shared.Contract.ResultPattern;

namespace IdentityModule.Feature.Vendors.GetVendorById
{
    internal class GetVendorByIdEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/vendors/{id}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new GetVendorByIdQuery(id));
                return result.Match(Results.Ok, Results.NotFound);
            }).WithTags("Vendors");
        }
    }
}
