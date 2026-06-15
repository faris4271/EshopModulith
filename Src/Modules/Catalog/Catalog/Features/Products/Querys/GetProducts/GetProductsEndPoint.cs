using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Shared.Contract.ResultPattern;
using Shared.Web.SmartTable;

namespace Catalog.Features.Products.Querys.GetProducts
{
    internal class GetProductsEndPoint : ICarterModule
    {

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("api/product-grid", async ([FromBody] SmartTableParam smartTableParam, [FromServices] ISender send) =>
            {
                var request = new GetProductQuery(smartTableParam);

                var respons = await send.Send(request);

                return respons.Match(Results.Ok, Results.NotFound);

            }).WithTags("Products")
               .WithName("CreateProduct");


        }
    }
}
