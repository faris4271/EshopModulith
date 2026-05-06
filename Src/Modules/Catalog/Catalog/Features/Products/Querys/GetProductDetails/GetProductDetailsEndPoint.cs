using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Shared.Contract.ResultPattern;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Features.Products.Querys.GetProductDetails
{
    internal class GetProductDetailsEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/get-product-details", async (Guid Id, [FromServices] ISender send) =>
            {
                var request = new GetProductDetailsQuery(Id);
                var respons = await send.Send(request);
                
                return respons.Match(Results.Ok, Results.BadRequest);

            }).WithTags("Products")
               .WithName("GetProductDetails");
        }
    }
}
