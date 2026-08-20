using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using MultiTenancyModule.Features.Tenants.SeedTenant;
using Shared.Contract.ResultPattern;

namespace MultiTenancyModule.Features.Tenants.SeedTenant
{
    public class SeedTenantEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("api/tenants/{tenantId}/seed", async (string tenantId, [FromServices] ISender sender) =>
            {
                var command = new SeedTenantCommand(tenantId);
                var result = await sender.Send(command);
                return result.Match(
                    () => Results.Ok(new { Message = "Tenant seeded successfully" }),
                    error => error.Type == ErrorType.NotFound
                        ? Results.NotFound(new { Error = error.Description })
                        : Results.BadRequest(new { Error = error.Description }));
            })
            .WithName("SeedTenant")
            .WithTags("Tenants")
            .RequireAuthorization("MultiTenancy.Tenants.Seed")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);
        }
    }
}