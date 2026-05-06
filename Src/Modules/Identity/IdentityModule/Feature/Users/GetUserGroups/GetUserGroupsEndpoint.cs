
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Identity.Contract.Feature.Users.GetUserGroups;
using Shared.Contract.ResultPattern;
using System.Net;

namespace IdentityModule.Feature.Users.GetUserGroups;

public class GetUserGroupsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
         app.MapGet("/users/{userId}/groups", async (string userId, ISender sender, CancellationToken cancellationToken) =>
       {
          var result=await sender.Send(new GetUserGroupsQuery(userId), cancellationToken);

          return result.Match(Results.Ok, Results.BadRequest);


           
        })
        .WithName("GetUserGroups")
        .WithSummary("Get groups for a user")
        .WithDescription("Retrieve all groups that a specific user belongs to.");
    }
}
