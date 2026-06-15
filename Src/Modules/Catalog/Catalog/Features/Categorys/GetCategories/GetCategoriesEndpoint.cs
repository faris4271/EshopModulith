using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Shared.Contract.ResultPattern;

namespace Catalog.Features.Categorys.GetCategories
{
    internal class GetCategoriesEndpoint : ICarterModule
    {

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("api/category", async ([FromServices] ISender send) =>
            {
                var query = new GetCategoriesQuery();
                var result = await send.Send(query);
                return result.Match(Results.Ok, Results.BadRequest);
            }).WithTags("Category")
               .WithName("GetCategories").AllowAnonymous();
        }
    }
}
