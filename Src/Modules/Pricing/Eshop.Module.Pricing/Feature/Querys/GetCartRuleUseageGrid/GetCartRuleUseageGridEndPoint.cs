using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Web.SmartTable;

namespace Eshop.Module.Pricing.Feature.Querys.GetCartRuleUseageGrid
{
    internal class GetCartRuleUseageGridEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/basket/cart-rules-usage/grid", async (ISender sender, HttpContext context) =>
            {
                using var reader = new StreamReader(context.Request.Body);
                var bodyString = await reader.ReadToEndAsync();
                var smartTableParam = Newtonsoft.Json.JsonConvert.DeserializeObject<SmartTableParam>(bodyString);

                var result = await sender.Send(new GetCartRuleUseageGridQuery(smartTableParam));
                return Results.Ok(result);
            }).WithName("GetCartRuleUseageGrid").WithTags("Basket");
        }
    }
}
