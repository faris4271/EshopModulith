using Carter;
using EShop.Module.Core.Contract.Dtos;
using EShop.Module.Core.Contract.Feature.Widgets.CreatWidget;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Contract.ResultPattern;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Features.Products.Commands.CreatProductWidget
{
    internal class CreatProductWidgetEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/catalog/product-widget", async (WidgetBaseDto widgetBase,ISender sender) =>
            {
                var command = new CreatWidgetInstanceCommand (widgetBase);
                var result = await sender.Send(command);
                return result.Match(Results.Created, Results.BadRequest);

            }).WithName("CreateProductWidget")
            .WithTags("product-widget"); ;
        }
    }
}
