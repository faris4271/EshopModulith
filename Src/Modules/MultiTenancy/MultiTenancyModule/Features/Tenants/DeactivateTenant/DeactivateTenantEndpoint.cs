using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using MultiTenancyModule.Features.Tenants.DeactivateTenant;
using Shared.Contract.ResultPattern;

namespace MultiTenancyModule.Features.Tenants.DeactivateTenant
{
    public class DeactivateTenantEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("api/tenants/{id}/deactivate", async (string id, [FromServices] ISender sender) =>
            {
                var command = new DeactivateTenantCommand(id);
                var result = await sender.Send(command);
                return result.Match(
                    tenantId => Results.Ok(new { Id = tenantId, Message = "Tenant deactivated successfully" }),
                    error => error.Type == ErrorType.NotFound
                        ? Results.NotFound(new { Error = error.Description })
                        : Results.BadRequest(new { Error = error.Description }));
            })
            .WithName("DeactivateTenant")
            .WithTags("Tenants")
            .RequireAuthorization("MultiTenancy.Tenants.Deactivate")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);
        }
    }
}