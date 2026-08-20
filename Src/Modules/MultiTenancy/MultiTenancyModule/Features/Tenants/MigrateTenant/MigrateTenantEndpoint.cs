using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using MultiTenancyModule.Features.Tenants.MigrateTenant;
using Shared.Contract.ResultPattern;

namespace MultiTenancyModule.Features.Tenants.MigrateTenant
{
    public class MigrateTenantEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("api/tenants/{tenantId}/migrate", async (string tenantId, [FromServices] ISender sender) =>
            {
                var command = new MigrateTenantCommand(tenantId);
                var result = await sender.Send(command);
                return result.Match(
                    () => Results.Ok(new { Message = "Tenant migrated successfully" }),
                    error => error.Type == ErrorType.NotFound
                        ? Results.NotFound(new { Error = error.Description })
                        : Results.BadRequest(new { Error = error.Description }));
            })
            .WithName("MigrateTenant")
            .WithTags("Tenants")
            .RequireAuthorization("MultiTenancy.Tenants.Migrate")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);
        }
    }
}