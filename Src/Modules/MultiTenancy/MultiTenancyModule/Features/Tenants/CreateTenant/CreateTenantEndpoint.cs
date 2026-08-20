using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using MultiTenancyModule.Contract.Dtos;
using Shared.Contract.ResultPattern;

namespace MultiTenancyModule.Features.Tenants.CreateTenant
{
    public class CreateTenantEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("api/tenants", async ([FromBody] CreateTenantDto dto, [FromServices] ISender sender) =>
            {
                var command = new CreateTenantCommand(
                    dto.Name,
                    dto.Identifier,
                    dto.ConnectionString,
                    dto.AdminEmail,
                    dto.Issuer,
                    dto.SubscriptionPlan ?? "default",
                    dto.TrialEndsAt ?? DateTime.UtcNow.AddMonths(1)
                );

                var result = await sender.Send(command);
                return result.Match(
                    id => Results.Created($"/api/tenants/{id}", new { Id = id }),
                    error => error.Type == ErrorType.Conflict
                        ? Results.Conflict(new { Error = error.Description })
                        : Results.BadRequest(new { Error = error.Description }));
            })
            .WithName("CreateTenant")
            .WithTags("Tenants")
            .RequireAuthorization("MultiTenancy.Tenants.Create")
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status409Conflict)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);
        }
    }
}