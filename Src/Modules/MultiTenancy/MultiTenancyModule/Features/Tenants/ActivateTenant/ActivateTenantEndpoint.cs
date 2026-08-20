using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Shared.Contract.ResultPattern;

namespace MultiTenancyModule.Features.Tenants.ActivateTenant
{
    public class ActivateTenantEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("api/tenants/{id}/activate", async (string id, [FromServices] ISender sender) =>
            {
                var command = new ActivateTenantCommand(id);
                var result = await sender.Send(command);
                return result.Match(
                    tenantId => Results.Ok(new { Id = tenantId, Message = "Tenant activated successfully" }),
                    error => error.Type == ErrorType.NotFound
                        ? Results.NotFound(new { Error = error.Description })
                        : error.Type == ErrorType.Conflict
                            ? Results.Conflict(new { Error = error.Description })
                            : Results.BadRequest(new { Error = error.Description }));
            })
            .WithName("ActivateTenant")
            .WithTags("Tenants")
            .RequireAuthorization("MultiTenancy.Tenants.Activate")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status409Conflict)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);
        }
    }
}