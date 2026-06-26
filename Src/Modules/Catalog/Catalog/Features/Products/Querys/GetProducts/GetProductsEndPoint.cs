using Carter;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
            app.MapPost("api/products/grid", async (HttpContext context, [FromServices] ISender send) =>
            {
                using var reader = new StreamReader(context.Request.Body);
                var bodyString = await reader.ReadToEndAsync();
                var smartTableParam = Newtonsoft.Json.JsonConvert.DeserializeObject<SmartTableParam>(bodyString);

                var request = new GetProductQuery(smartTableParam);

                var respons = await send.Send(request);

                return respons.Match(Results.Ok, Results.NotFound);

            }).WithTags("Products")
               .WithName("CreateProduct")
               .RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" });


        }
    }
}
