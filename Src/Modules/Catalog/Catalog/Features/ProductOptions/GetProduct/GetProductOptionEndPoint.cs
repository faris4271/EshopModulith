using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Contract.ResultPattern;

namespace Catalog.Features.ProductOptions.GetProduct
{
    internal class GetProductOptionEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("api/product-options/{id:guid}", async (ISender sender, Guid id, CancellationToken cancellationToken) =>
             {
                 var result = await sender.Send(new GetProductOptionQuery(id), cancellationToken);
                 return result.Match(Results.Ok, Results.BadRequest);
             }).WithTags("ProductOptions").AllowAnonymous();
        }
    }
}
