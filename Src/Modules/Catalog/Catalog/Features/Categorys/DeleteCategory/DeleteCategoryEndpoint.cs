using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Shared.Contract.ResultPattern;

namespace Catalog.Features.Categorys.DeleteCategory
{
    public class DeleteCategoryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("api/category/{id}", async (Guid id, [FromServices] ISender sender) =>
            {
                var command = new DeleteCategoryCommand(id);
                var result = await sender.Send(command);
                return result.Match(Results.Ok, Results.BadRequest);
            }).WithTags("Category").AllowAnonymous();
        }
    }
}
