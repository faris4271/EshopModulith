using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using MultiTenancyModule.Contract.Dtos;
using MultiTenancyModule.Features.Tenants.GetTenantSettings;
using Shared.Contract.ResultPattern;

namespace MultiTenancyModule.Features.Tenants.GetTenantSettings
{
    public class GetTenantSettingsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("api/tenants/{tenantId:guid}/settings", async (Guid tenantId, [FromServices] ISender sender) =>
            {
                var query = new GetTenantSettingsQuery(tenantId);
                var result = await sender.Send(query);
                return result.Match(
                    settings => Results.Ok(settings),
                    error => error.Type == ErrorType.NotFound
                        ? Results.NotFound(new { Error = error.Description })
                        : Results.BadRequest(new { Error = error.Description }));
            })
            .WithName("GetTenantSettings")
            .WithTags("Tenants")
            .RequireAuthorization("MultiTenancy.Tenants.ReadSettings")
            .Produces<TenantSettingsDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);
        }
    }
}