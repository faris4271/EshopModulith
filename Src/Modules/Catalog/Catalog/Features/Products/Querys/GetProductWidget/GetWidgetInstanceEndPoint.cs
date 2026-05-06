using Carter;
using EShop.Module.Core.Contract.Feature.Widgets.GetProductWidgetInstances;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Contract.ResultPattern;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Features.Products.Querys.GetProductWidget
{
    internal class GetWidgetInstanceEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/catalog/product-widget/{id}", async (Guid id, ISender sender) =>
            {
                var query = new GetWidgetInstanceQuery(id);
                var result = await sender.Send(query);
                return result.Match(Results.Ok, Results.NotFound);
            }).WithName("GetProductWidget")
            .WithTags("product-widget");
        }
    }
}
