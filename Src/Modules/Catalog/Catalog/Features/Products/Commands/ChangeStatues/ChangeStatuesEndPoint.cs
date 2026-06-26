using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Catalog.Features.Products.Commands.ChangeStatues
{
    internal class ChangeStatuesEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("api/products/{id}/toggle-status", async (Guid id, ISender sender) =>
            {
                var command = new ChangeStatuesCommand(id);
                var result = await sender.Send(command);
                return result.Match(Results.NoContent, Results.BadRequest);
            }).WithTags("product");
        }
    }
}
