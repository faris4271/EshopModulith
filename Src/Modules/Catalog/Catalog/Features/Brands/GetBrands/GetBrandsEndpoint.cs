using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Contract.ResultPattern;


namespace Catalog.Features.Brands.GetBrands
{
    internal class GetBrandsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("api/brands", async (ISender sender) =>
            {
                var brands = await sender.Send(new GetBrandsQuery());

                return brands.Match(Results.Ok, Results.BadRequest);

            }).WithTags("Brand").AllowAnonymous();
        }
    }
}
