using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Catalog.Features.ProductTemplet.CreatProductTemplet
{
    internal class CreatProductTempleEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("api/product-templet", async (ISender sender, CreatProductTempletCommand command) =>
            {
                var result = await sender.Send(command);

                return result.Match(Results.Created, Results.BadRequest);
            }).WithTags("Product-Templet");
        }
    }
}
