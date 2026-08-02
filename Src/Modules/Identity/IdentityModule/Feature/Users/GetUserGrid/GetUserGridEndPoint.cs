using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Identity.Contract.Feature.Users.GetUserGrid;
using Shared.Contract.ResultPattern;
using Shared.Web.SmartTable;

namespace IdentityModule.Feature.Users.GetUserGrid
{
    internal class GetUserGridEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/users/grid", async (HttpContext context, ISender sender) =>
            {
                using var reader = new StreamReader(context.Request.Body);
                var bodyString = await reader.ReadToEndAsync();
                var smartTableParam = Newtonsoft.Json.JsonConvert.DeserializeObject<SmartTableParam>(bodyString);
                var query = new GetUserGridQuery(smartTableParam);
                var result = await sender.Send(query);
                return result.Match(
                    success => Results.Ok(success),
                    errors => Results.BadRequest(errors));
            }).WithTags("Users");
        }
    }
}
