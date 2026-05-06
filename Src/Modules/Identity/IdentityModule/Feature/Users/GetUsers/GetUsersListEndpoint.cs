using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Identity.Contract.Feature.Users.GetUsers;
using Shared.Contract.ResultPattern;

namespace IdentityModule.Feature.Users.GetUsers;

public  class GetUsersListEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/users", async (CancellationToken cancellationToken, ISender sender) =>
           { 
              var result=await sender.Send(new GetUsersQuery(), cancellationToken); 

               result.Match(Results.Ok,Results.BadRequest);
           })
       .WithName("ListUsers")
       .WithSummary("List users")
       //.RequirePermission(IdentityPermissionConstants.Users.View)
       .WithDescription("Retrieve a list of users for the current tenant.");
    }
}
