using Carter;
using Catalog.Features.ProductOptions.CreateProductOption;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Catalog.Features.ProductOptions.CreatProductOption
{
    internal class CreatProductOptionEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("api/product-options", async (ISender sender, CreateProductOptionCommand command, CancellationToken cancellationToken) =>
            {


                var result = await sender.Send(command, cancellationToken);

                return result.Match(Results.Created, Results.BadRequest);
            }).WithTags("ProductOptions").AllowAnonymous();
        }
    }
}
