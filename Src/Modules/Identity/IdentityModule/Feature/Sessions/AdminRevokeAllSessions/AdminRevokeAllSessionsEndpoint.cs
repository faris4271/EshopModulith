using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Identity.Contract.Feature.Sessions.AdminRevokeAllSessions;
using Shared.Contract.ResultPattern;
using System.Net;

namespace IdentityModule.Feature.Sessions.AdminRevokeAllSessions;

public  class AdminRevokeAllSessionsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
         app.MapPost("/users/{userId:guid}/sessions/revoke-all", async (Guid userId, AdminRevokeAllSessionsCommand? command, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(command ?? new AdminRevokeAllSessionsCommand(userId), cancellationToken);
           
            return result.Match(Results.Ok,Results.BadRequest);


        })
        .WithName("AdminRevokeAllSessions")
        .WithSummary("Revoke all user's sessions (Admin)")
        //.RequirePermission(IdentityPermissionConstants.Sessions.RevokeAll)
        .WithDescription("Revoke all sessions for a specific user. Requires admin permission.");
    }
}
