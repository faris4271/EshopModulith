

using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Identity.Contract.Feature.Sessions.GetUserSessions;
using Shared.Contract.ResultPattern;

namespace IdentityModule.Feature.Sessions.GetUserSessions;

public class GetUserSessionsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
         app.MapGet("/users/{userId:guid}/sessions", async (Guid userId, CancellationToken cancellationToken, ISender sender) =>
         {
           var result=await  sender.Send(new GetUserSessionsQuery(userId), cancellationToken);

             return result.Match(
                 Results.Ok,
                 Results.NotFound
                );
         })
        .WithName("GetUserSessions")
        .WithSummary("Get user's sessions (Admin)")
        //.RequirePermission(IdentityPermissionConstants.Sessions.ViewAll)
        .WithDescription("Retrieve all active sessions for a specific user. Requires admin permission.");
    }
}
