using Carter;
using Catalog.Category.Dtos;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Catalog.Features.Categorys.CreatCategory
{
    public class CreatCategoryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/category", async ([FromForm] CategoryDto dto, [FromServices] ISender sender) =>
            {
                var command = new CreatCategoryCommand(dto);
                var result = await sender.Send(command);
                return result.Match(Results.Created, Results.BadRequest);
            }).WithTags("Category").DisableAntiforgery();
        }
    }
}
