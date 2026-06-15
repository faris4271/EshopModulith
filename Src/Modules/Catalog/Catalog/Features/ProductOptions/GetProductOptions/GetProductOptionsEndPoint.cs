using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Contract.ResultPattern;

namespace Catalog.Features.ProductOptions.GetProductOptions
{
    internal class GetProductOptionsEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("api/product-options", async (ISender sender, CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(new GetProductOptionsQuery(), cancellationToken);

                return result.Match(Results.Ok, Results.BadRequest);


            }).WithTags("ProductOptions").AllowAnonymous();
        }
    }
}
