using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using MultiTenancyModule.Contract.Dtos;
using Shared.Contract.ResultPattern;

namespace MultiTenancyModule.Features.Tenants.GetTenantTheme
{
    public class GetTenantThemeEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/tenants").WithTags("Tenants").RequireAuthorization("MultiTenancy.Tenants.Read");

            group.MapGet("{tenantId}/theme", async (string tenantId, [FromServices] ISender sender) =>
            {
                var query = new GetTenantThemeQuery(tenantId);
                var result = await sender.Send(query);
                return result.Match(
                    theme => Results.Ok(theme),
                    error => error.Type == ErrorType.NotFound
                        ? Results.NotFound(new { Error = error.Description })
                        : Results.BadRequest(new { Error = error.Description }));
            })
            .WithName("GetTenantTheme")
            .Produces<TenantThemeDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest);

            group.MapGet("theme/current", async ([FromServices] ISender sender) =>
            {
                var query = new GetCurrentTenantThemeQuery();
                var result = await sender.Send(query);
                return result.Match(
                    theme => Results.Ok(theme),
                    error => error.Type == ErrorType.NotFound
                        ? Results.NotFound(new { Error = error.Description })
                        : Results.BadRequest(new { Error = error.Description }));
            })
            .WithName("GetCurrentTenantTheme")
            .Produces<TenantThemeDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest);

            group.MapGet("theme/default", async ([FromServices] ISender sender) =>
            {
                var query = new GetDefaultTenantThemeQuery();
                var result = await sender.Send(query);
                return result.Match(
                    theme => Results.Ok(theme),
                    error => error.Type == ErrorType.NotFound
                        ? Results.NotFound(new { Error = error.Description })
                        : Results.BadRequest(new { Error = error.Description }));
            })
            .WithName("GetDefaultTenantTheme")
            .Produces<TenantThemeDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest);
        }
    }
}