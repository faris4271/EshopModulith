using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Shared.Contract.ResultPattern;

namespace Catalog.Features.Categorys.GetCategory
{
    public class GetCategoryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("api/category/{id}", async (Guid id, [FromServices] ISender sender) =>
            {
                var result = await sender.Send(new GetCategoryQuery(id));
                return result.Match(
                  Results.Ok,
                  Results.NotFound
                );
            }).WithTags("Category").AllowAnonymous();
        }
    }
}
