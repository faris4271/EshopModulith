using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Identity.Contract.Feature.Vendors;
using Shared.Contract.ResultPattern;
using Shared.Web.SmartTable;

namespace IdentityModule.Feature.Vendors.GetVendors
{
    internal class GetVenorsGridEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/vendors/grid", async (ISender sender, HttpContext context) =>
            {
                using var reader = new StreamReader(context.Request.Body);
                var bodyString = await reader.ReadToEndAsync();
                var smartTableParam = Newtonsoft.Json.JsonConvert.DeserializeObject<SmartTableParam>(bodyString);

                var result = await sender.Send(new GetVenorsGridQuery(smartTableParam));

                return result.Match(Results.Ok, Results.BadRequest);
            }).WithTags("Vendors");
        }
    }
}
