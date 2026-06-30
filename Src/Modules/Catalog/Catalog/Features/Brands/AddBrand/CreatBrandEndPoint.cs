using Carter;
using CatalogContract.Dtos;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Catalog.Features.Brands.AddBrand
{
    public class CreatBrandEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("Api/brands", async ([FromServices] ISender sender, CreatBrandDto dto) =>
            {
                var result = await sender.Send(new CreatBrandCommand(dto));

                result.Match(Results.Created, Results.BadRequest);

            }).WithTags("Brand").AllowAnonymous();
        }
    }
}
