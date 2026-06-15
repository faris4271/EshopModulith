using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Module.Identity.Contract.Services;
using Shared.Contract.ResultPattern;

namespace Eshop.Module.Basket.Feature.Querys.GetCartList
{
    internal class GetCartListEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/cart", async ([FromServices] ISender sender, [FromServices] ICurrentUserService currentUserService) =>
            {
                var currentUser = currentUserService.GetUserId();

                var result = await sender.Send(new GetCartListQuery(currentUser));
                return result.Match(Results.Ok, Results.BadRequest);
            });
        }
    }
}
