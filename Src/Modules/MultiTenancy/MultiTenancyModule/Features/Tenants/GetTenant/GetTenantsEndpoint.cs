using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using MultiTenancyModule.Contract.Dtos;
using MultiTenancyModule.Features.Tenants.GetTenant;
using Shared.Web.SmartTable;
using Shared.Contract.ResultPattern;

namespace MultiTenancyModule.Features.Tenants.GetTenant
{
    public class GetTenantsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("api/tenants/query", async ([FromBody] SmartTableParam param, [FromServices] ISender sender) =>
            {
                var query = new GetTenantsQuery(param);
                var result = await sender.Send(query);
                return result.Match(
                    data => Results.Ok(data),
                    error => Results.BadRequest(new { Error = error.Description }));
            })
            .WithName("GetTenants")
            .WithTags("Tenants")
            .RequireAuthorization("MultiTenancy.Tenants.Read")
            .Produces<SmartTableResult<TenantDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);
        }
    }
}