using Carter;
using Mapster;
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
        public record GetCategoriesRequest(int PageNumber, int PageSize);
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/categorys", async ([FromBody] GetCategoriesRequest request, [FromServices] ISender send) =>
            {
                var query = request.Adapt<GetCategoriesQuery>();
                var result = await send.Send(query);
                return result.Match(Results.Ok, Results.BadRequest);
            }).WithTags("Category")
               .WithName("GetCategories");
        }
    }
}
