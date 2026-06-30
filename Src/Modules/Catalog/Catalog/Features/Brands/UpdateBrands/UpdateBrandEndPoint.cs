using Carter;
using CatalogContract.Dtos;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Catalog.Features.Brands.UpdateBrands
{
    internal class UpdateBrandEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("api/brands/{id}", async (Guid id, CreatBrandDto brandDto, [FromServices] ISender sender) =>
            {
                var command = new UpdateBrandCommand(id, brandDto);

                var result = await sender.Send(command);

                return result.Match(Results.NoContent, Results.BadRequest);
            }).WithTags("Brand").AllowAnonymous();
        }
    }
}
