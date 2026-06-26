using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Catalog.Features.ProductTemplet.DeleteProductTemplet
{
    internal class DeleteProductTempletEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("api/product-templet/{id:guid}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new DeleteProductTempletCommand(id));

                return result.Match(Results.NoContent, Results.BadRequest);

            }).WithTags("Product-Templet");
        }
    }
}