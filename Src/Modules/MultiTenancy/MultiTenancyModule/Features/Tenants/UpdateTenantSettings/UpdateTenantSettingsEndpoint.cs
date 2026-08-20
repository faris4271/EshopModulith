using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using MultiTenancyModule.Contract.Dtos;
using MultiTenancyModule.Features.Tenants.UpdateTenantSettings;
using Shared.Contract.ResultPattern;

namespace MultiTenancyModule.Features.Tenants.UpdateTenantSettings
{
    public class UpdateTenantSettingsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("api/tenants/{tenantId:guid}/settings", async (Guid tenantId, [FromBody] UpdateTenantSettingsDto dto, [FromServices] ISender sender) =>
            {
                var command = new UpdateTenantSettingsCommand(tenantId, dto);
                var result = await sender.Send(command);
                return result.Match(
                    settings => Results.Ok(settings),
                    error => error.Type == ErrorType.NotFound
                        ? Results.NotFound(new { Error = error.Description })
                        : Results.BadRequest(new { Error = error.Description }));
            })
            .WithName("UpdateTenantSettings")
            .WithTags("Tenants")
            .RequireAuthorization("MultiTenancy.Tenants.UpdateSettings")
            .Produces<TenantSettingsDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);
        }
    }
}