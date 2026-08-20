using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using MultiTenancyModule.Features.Tenants.AdjustTenantValidity;
using Shared.Contract.ResultPattern;

namespace MultiTenancyModule.Features.Tenants.AdjustTenantValidity
{
    public class AdjustTenantValidityEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("api/tenants/{id}/validity", async (string id, [FromBody] AdjustTenantValidityDto dto, [FromServices] ISender sender) =>
            {
                var command = new AdjustTenantValidityCommand(id, dto.ValidUpto);
                var result = await sender.Send(command);
                return result.Match(
                    validUpto => Results.Ok(new { ValidUpto = validUpto }),
                    error => error.Type == ErrorType.NotFound
                        ? Results.NotFound(new { Error = error.Description })
                        : Results.BadRequest(new { Error = error.Description }));
            })
            .WithName("AdjustTenantValidity")
            .WithTags("Tenants")
            .RequireAuthorization("MultiTenancy.Tenants.AdjustValidity")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);
        }
    }

    public record AdjustTenantValidityDto(DateTime ValidUpto);
}