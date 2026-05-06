using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Identity.Contract.Feature.Roles.GetRoleWithPermissions;
using Shared.Contract.ResultPattern;
using System.Net;

namespace IdentityModule.Feature.Roles.GetRoleWithPermissions;

public class GetRolePermissionsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/{id:guid}/permissions", async (string id, ISender sender, CancellationToken cancellationToken) =>
          { 
            var result=await  sender.Send(new GetRoleWithPermissionsQuery(id), cancellationToken);

              return result.Match(Results.Ok,Results.BadRequest);
          })
        .WithName("GetRolePermissions")
        .WithSummary("Get role permissions")
        //.RequirePermission(IdentityPermissionConstants.Roles.View)
        .WithDescription("Retrieve a role along with its assigned permissions.");
    }
}
