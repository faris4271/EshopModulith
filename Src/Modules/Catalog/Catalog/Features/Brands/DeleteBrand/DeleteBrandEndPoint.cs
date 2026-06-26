using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Catalog.Features.Brands.DeleteBrand
{
    internal class DeleteBrandEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("api/brands/{Id}", async ([FromServices] ISender sender, Guid Id) =>
            {
                var result = await sender.Send(new DeleteBrandCommand(Id));

                return result.Match(Results.NoContent, Results.BadRequest);
            }).WithTags("Brand");
        }
    }
}
