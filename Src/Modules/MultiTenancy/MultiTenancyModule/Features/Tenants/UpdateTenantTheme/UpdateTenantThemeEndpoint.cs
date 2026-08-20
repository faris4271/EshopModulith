using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using MultiTenancyModule.Contract.Dtos;
using MultiTenancyModule.Features.Tenants.UpdateTenantTheme;
using Shared.Contract.ResultPattern;

namespace MultiTenancyModule.Features.Tenants.UpdateTenantTheme
{
    public class UpdateTenantThemeEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("api/tenants/{tenantId}/theme", async (string tenantId, [FromBody] TenantThemeDto theme, [FromServices] ISender sender) =>
            {
                var command = new UpdateTenantThemeCommand(tenantId, theme);
                var result = await sender.Send(command);
                return result.Match(
                    () => Results.Ok(new { Message = "Theme updated successfully" }),
                    error => error.Type == ErrorType.NotFound
                        ? Results.NotFound(new { Error = error.Description })
                        : Results.BadRequest(new { Error = error.Description }));
            })
            .WithName("UpdateTenantTheme")
            .WithTags("Tenants")
            .RequireAuthorization("MultiTenancy.Tenants.UpdateTheme")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);
        }
    }
}