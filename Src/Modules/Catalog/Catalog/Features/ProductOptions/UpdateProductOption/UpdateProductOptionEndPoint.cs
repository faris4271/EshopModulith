using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Contract.ResultPattern;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Features.ProductOptions.UpdateProductOption
{
    internal class UpdateProductOptionEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("api/product-options", async (ISender sender, UpdateProductOptionCommand command, CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(command, cancellationToken);
                return result.Match(Results.NoContent, Results.BadRequest);
            }).WithTags("ProductOptions");
        }
    }
}
