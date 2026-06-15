using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Shared.Contract.ResultPattern;

namespace Catalog.Features.Brands.GetBrandById
{
    internal class GetBrandByIEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("api/brands/{Id}", async ([FromServices] ISender sender, Guid Id) =>
            {
                var result = await sender.Send(new GetBrandByIdQuery(Id));

                return result.Match(Results.Ok, Results.BadRequest);
            }).WithTags("Brand").AllowAnonymous();
        }
    }
}
