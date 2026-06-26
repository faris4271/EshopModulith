using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Contract.ResultPattern;

namespace Catalog.Features.ProductTemplet.GetProductTemplet
{
    internal class GetTempletEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("api/product-templet/{id:guid}", async (ISender sender, Guid id) =>
            {
                var result = await sender.Send(new GetTempletQuery(id));

                return result.Match(Results.Ok, Results.NotFound);
            }).WithTags("Product-Templet");
        }
    }
}
