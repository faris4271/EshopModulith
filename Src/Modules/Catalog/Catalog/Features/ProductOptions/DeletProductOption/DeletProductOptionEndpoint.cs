using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Features.ProductOptions.DeletProductOption
{
    internal class DeletProductOptionEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("api/product-options/{id:guid}", async (ISender sender, Guid id, CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(new DeletProductOptionCommand(id), cancellationToken);
                return result.Match(Results.NoContent, Results.BadRequest);
            }).WithTags("ProductOptions"); ;
        }
    }
}
