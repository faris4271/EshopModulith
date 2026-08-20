using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using MultiTenancyModule.Contract.Dtos;
using MultiTenancyModule.Features.Tenants.GetTenantStatus;
using Shared.Contract.ResultPattern;

namespace MultiTenancyModule.Features.Tenants.GetTenantStatus
{
    public class GetTenantStatusEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("api/tenants/{id}/status", async (string id, [FromServices] ISender sender) =>
            {
                var query = new GetTenantStatusQuery(id);
                var result = await sender.Send(query);
                return result.Match(
                    status => Results.Ok(status),
                    error => error.Type == ErrorType.NotFound
                        ? Results.NotFound(new { Error = error.Description })
                        : Results.BadRequest(new { Error = error.Description }));
            })
            .WithName("GetTenantStatus")
            .WithTags("Tenants")
            .RequireAuthorization("MultiTenancy.Tenants.Read")
            .Produces<TenantStatusDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);
        }
    }
}