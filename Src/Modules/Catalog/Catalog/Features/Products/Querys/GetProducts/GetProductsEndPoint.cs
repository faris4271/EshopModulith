using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Catalog.Features.Products.Querys.GetProducts
{
    internal class GetProductsEndPoint : ICarterModule
    {
        public record GetProductRequest(int PageNumber, int PageSize);
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/get-product", async (int PageNumber,int PageSize,[FromServices] ISender send) =>
            {
                var request = new GetProductQuery { PageNumber = PageNumber, PageSize = PageSize };

                var respons = await send.Send(request);

                if (!respons.IsSuccess)
                    return Results.BadRequest(respons.Error);

                return Results.Ok(respons.Value);

            }).WithTags("Products")
               .WithName("CreateProduct");


        }
    }
}
