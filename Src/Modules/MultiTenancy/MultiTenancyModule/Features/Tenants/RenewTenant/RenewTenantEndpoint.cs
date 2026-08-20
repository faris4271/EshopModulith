using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using MultiTenancyModule.Features.Tenants.RenewTenant;
using Shared.Contract.ResultPattern;

namespace MultiTenancyModule.Features.Tenants.RenewTenant
{
    public class RenewTenantEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("api/tenants/{id}/renew", async (string id, [FromBody] RenewTenantDto dto, [FromServices] ISender sender) =>
            {
                var command = new RenewTenantCommand(id, dto.NewPlanKey, dto.TermMonths);
                var result = await sender.Send(command);
                return result.Match(
                    r => Results.Ok(new { r.PeriodStartUtc, r.ValidUpto, r.PlanChanged }),
                    error => error.Type == ErrorType.NotFound
                        ? Results.NotFound(new { Error = error.Description })
                        : Results.BadRequest(new { Error = error.Description }));
            })
            .WithName("RenewTenant")
            .WithTags("Tenants")
            .RequireAuthorization("MultiTenancy.Tenants.Renew")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);
        }
    }

    public record RenewTenantDto(string NewPlanKey, int TermMonths);
}