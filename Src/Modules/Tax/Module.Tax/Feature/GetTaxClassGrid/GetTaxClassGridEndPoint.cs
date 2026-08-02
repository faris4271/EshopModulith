using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Contract.ResultPattern;
using Shared.Web.SmartTable;

namespace Module.Tax.Feature.GetTaxClassGrid
{
    internal class GetTaxClassGridEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/tax/classes/grid", async (HttpContext context, ISender sender, CancellationToken ct) =>
            {
                using var reader = new StreamReader(context.Request.Body);
                var bodyString = await reader.ReadToEndAsync();
                var smartTableParam = Newtonsoft.Json.JsonConvert.DeserializeObject<SmartTableParam>(bodyString);

                var query = new GetTaxClassGridQuery(smartTableParam);
                var result = await sender.Send(query, ct);
                return result.Match(
                  Results.Ok,
                  Results.BadRequest
                );
            }).WithTags("Tax");
        }
    }
}
