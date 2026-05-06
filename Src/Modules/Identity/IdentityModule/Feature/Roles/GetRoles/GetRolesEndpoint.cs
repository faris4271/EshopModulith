using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Identity.Contract.Feature.Roles.GetRoles;

namespace IdentityModule.Feature.Roles.GetRoles;

public static class GetRolesEndpoint
{
    public static RouteHandlerBuilder MapGetRolesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/roles", (ISender sender, CancellationToken cancellationToken) =>
            sender.Send(new GetRolesQuery(), cancellationToken))
        .WithName("ListRoles")
        .WithSummary("List all roles")
        //.RequirePermission(IdentityPermissionConstants.Roles.View)
        .WithDescription("Retrieve all roles available for the current tenant.");
    }
}
