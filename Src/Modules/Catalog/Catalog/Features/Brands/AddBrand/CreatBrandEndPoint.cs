using Carter;
using Catalog.Brands.Dtos;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Features.Brands.AddBrand
{
    public class CreatBrandEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("Api/Brand", async ([FromServices]ISender sender, CreatBrandDto dto) =>
            {
                var result=await sender.Send(new CreatBrandCommand(dto));

                result.Match(Results.Created, Results.BadRequest);

            });
        }
    }
}
