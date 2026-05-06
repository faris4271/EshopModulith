using Carter;
using EShop.Module.Core.Contract.Dtos;
using EShop.Module.Core.Contract.Feature.Widgets.UpdateWidgetInstance;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Contract.ResultPattern;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Features.Products.Commands.UpdateProductWidget
{
    public class UpdateProductWidgetEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/api/catalog/product-widget{id}", async (Guid id,WidgetBaseDto widgetBase, ISender sender) =>
            {
                var command = new UpdateWidgetInstanceCommand(id,widgetBase);
                var result = await sender.Send(command);
                return result.Match(Results.NoContent, Results.BadRequest);
            }).WithName("UpdateProductWidget")
            .WithTags("product-widget"); ;
        }
    }
}
