
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Identity.Contract.Feature.Roles.DeleteRole;
using System.Net;

namespace IdentityModule.Feature.Roles.DeleteRole;

public  class DeleteRoleEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
         app.MapDelete("/roles/{id:guid}", async (string id, ISender sender, CancellationToken cancellationToken) =>
        {
            await sender.Send(new DeleteRoleCommand(id), cancellationToken);
            return TypedResults.NoContent();
        })
     .WithName("DeleteRole")
     .WithSummary("Delete role by ID")
     //.RequirePermission(IdentityPermissionConstants.Roles.Delete)
     .WithDescription("Remove an existing role by its unique identifier.");
    }
}
