using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Catalog.Features.ProductTemplet.UpdateProductTemplet
{
    internal class UpdateProductTempletEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("api/product-templet/{id:guid}", async (ISender sender, Guid id, UpdateProductTempletCommand command) =>
            {
                if (id != command.TempletDto.Id)
                {
                    return Results.BadRequest("Id in route does not match Id in command.");
                }
                var result = await sender.Send(command);

                return result.Match(Results.NoContent, Results.BadRequest);

            }).WithTags("Product-Templet");
        }
    }
}