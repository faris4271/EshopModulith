using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using MultiTenancyModule.Contract.Dtos;
using MultiTenancyModule.Features.Tenants.UpdateTenant;
using Shared.Contract.ResultPattern;

namespace MultiTenancyModule.Features.Tenants.UpdateTenant
{
    public class UpdateTenantEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("api/tenants/{id}", async (string id, [FromBody] UpdateTenantDto dto, [FromServices] ISender sender) =>
            {
                var command = new UpdateTenantCommand(
                    id,
                    dto.Name,
                    dto.ConnectionString,
                    dto.SubscriptionPlan,
                    dto.TrialEndsAt,
                    dto.SubscriptionEndsAt
                );

                var result = await sender.Send(command);
                return result.Match(
                    tenantId => Results.Ok(new { Id = tenantId }),
                    error => error.Type == ErrorType.NotFound
                        ? Results.NotFound(new { Error = error.Description })
                        : Results.BadRequest(new { Error = error.Description }));
            })
            .WithName("UpdateTenant")
            .WithTags("Tenants")
            .RequireAuthorization("MultiTenancy.Tenants.Update")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);
        }
    }
}