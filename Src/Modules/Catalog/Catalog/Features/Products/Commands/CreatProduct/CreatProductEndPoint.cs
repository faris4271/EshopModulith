using Carter;
using Catalog.Products.Dtos;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using static Catalog.Features.Products.Commands.CreatProduct.CreatProductCommandHandler;

namespace Catalog.Features.Products.Commands.CreatProduct
{
    public class CreatProductEndPoint : ICarterModule
    {

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/product", async ([FromForm] ProductForm productRecuest, [FromServices] ISender send) =>
            {
                var command = new CreatProductCommand(productRecuest);

                var respons = await send.Send(command);

                return respons.Match(Results.Created, Results.BadRequest);
            }).WithTags("product").DisableAntiforgery();

        }
    }
}
