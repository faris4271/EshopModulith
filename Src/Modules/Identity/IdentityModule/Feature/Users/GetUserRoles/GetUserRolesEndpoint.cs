using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Identity.Contract.Feature.Users.GetUserRoles;
using Shared.Contract.ResultPattern;

namespace IdentityModule.Feature.Users.GetUserRoles;

public class GetUserRolesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {

        app.MapGet("/users/{id:guid}/roles", async (string id, ISender sender, CancellationToken cancellationToken) => 
        {
           
           var result=await sender.Send(new GetUserRolesQuery(id), cancellationToken);

            result.Match(Results.Ok, Results.BadRequest);
         })
       .WithName("GetUserRoles")
       .WithSummary("Get user roles")
       //.RequirePermission(IdentityPermissionConstants.Users.View)
       .WithDescription("Retrieve the roles assigned to a specific user.");
    }
}
