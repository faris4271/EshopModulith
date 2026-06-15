using Carter;
using Eshop.Module.Basket.Contract.Dtos;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Eshop.Module.Basket.Feature.Commands.CreatCartRule
{
    internal class CreatCartRuleEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/basket/cartRule", async ([FromBody] CartRuleDto cartRuleDto, [FromServices] ISender sender) =>
            {
                var command = new CreatCartRuleCommand(cartRuleDto);
                var result = await sender.Send(command);
                return result.Match(
                    Results.Created,
                    Results.BadRequest
                );
            }).WithTags("Basket");
        }
    }
}
