using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Contract.ResultPattern;
using Shared.Web.SmartTable;

namespace Eshop.Module.Pricing.Feature.Querys.GetCartRuleGrid
{
    internal class GetCartRuleGridEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/basket/cart-rules/grid", async (HttpContext context, ISender sender, CancellationToken ct) =>
            {
                using var reader = new StreamReader(context.Request.Body);
                var bodyString = await reader.ReadToEndAsync();
                var smartTableParam = Newtonsoft.Json.JsonConvert.DeserializeObject<SmartTableParam>(bodyString);

                var result = await sender.Send(new GetCartRuleGridQuery(smartTableParam), ct);
                return result.Match(Results.Ok, Results.NotFound);
            }).WithTags("Cart");
        }
    }
}
