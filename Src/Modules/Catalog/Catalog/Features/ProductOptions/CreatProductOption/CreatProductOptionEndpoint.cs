using Carter;
using Catalog.Features.ProductOptions.CreateProductOption;
using CatalogContract.Dtos;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Features.ProductOptions.CreatProductOption
{
    internal class CreatProductOptionEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("api/product-options", async (ISender sender, CreatProductOptionDto productOptionDto, CancellationToken cancellationToken) =>
            {
                var command = new CreateProductOptionCommand(productOptionDto);

                var result = await sender.Send(command, cancellationToken);

                return result.Match(Results.Created, Results.BadRequest);
            }).WithTags("ProductOptions");
        }
    }
}
