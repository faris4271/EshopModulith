using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using MultiTenancyModule.Features.Tenants.DeleteTenant;
using Shared.Contract.ResultPattern;

namespace MultiTenancyModule.Features.Tenants.DeleteTenant
{
    public class DeleteTenantEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("api/tenants/{id}", async (string id, [FromServices] ISender sender) =>
            {
                var command = new DeleteTenantCommand(id);
                var result = await sender.Send(command);
                return result.Match(
                    tenantId => Results.Ok(new { Id = tenantId, Message = "Tenant deleted successfully" }),
                    error => error.Type == ErrorType.NotFound
                        ? Results.NotFound(new { Error = error.Description })
                        : error.Type == ErrorType.Forbidden
                            ? Results.Forbid()
                            : Results.BadRequest(new { Error = error.Description }));
            })
            .WithName("DeleteTenant")
            .WithTags("Tenants")
            .RequireAuthorization("MultiTenancy.Tenants.Delete")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status401Unauthorized);
        }
    }
}