using Carter;
using Catalog.Category.Dtos;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Catalog.Features.Categorys.UpdateCategory
{
    public class UpdateCategoryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("api/category/{id}", async (Guid id, [FromForm] CategoryDto dto, [FromServices] ISender sender) =>
            {
                var command = new UpdateCategoryCommand(dto, id);
                var result = await sender.Send(command);
                return result.Match(Results.NoContent, Results.BadRequest);
            }).WithTags("Category").DisableAntiforgery().AllowAnonymous();
        }
    }
}
