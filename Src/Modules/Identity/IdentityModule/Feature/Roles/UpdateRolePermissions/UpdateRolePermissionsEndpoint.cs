using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Module.Identity.Contract.Feature.Roles.UpdatePermissions;
using Shared.Contract.ResultPattern;
using System.Net;

namespace IdentityModule.Feature.Roles.UpdateRolePermissions;

public  class UpdateRolePermissionsEndpoint:ICarterModule
{
   public void AddRoutes(IEndpointRouteBuilder app)
    {
         app.MapPut("/{id}/permissions", Handler)
       .WithName("UpdateRolePermissions")
       .WithSummary("Update role permissions")
       //.RequirePermission(IdentityPermissionConstants.Roles.Update)
       .WithDescription("Replace the set of permissions assigned to a role.");
    }

    private static async Task<IResult> Handler(
        string id,
        [FromBody] UpdatePermissionsCommand request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        if (id != request.RoleId)
        {
            return TypedResults.BadRequest();
        }

        var response = await sender.Send(request, cancellationToken);
       return response.Match(Results.Ok,Results.BadRequest);
    }

   
}
