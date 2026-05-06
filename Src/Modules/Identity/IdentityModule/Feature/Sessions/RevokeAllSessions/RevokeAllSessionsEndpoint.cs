
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Identity.Contract.Feature.Sessions.RevokeAllSessions;
using Shared.Contract.ResultPattern;
using System.Net;

namespace IdentityModule.Feature.Sessions.RevokeAllSessions;

public  class RevokeAllSessionsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {

         app.MapPost("/sessions/revoke-all", async (RevokeAllSessionsCommand? command, ISender sender, CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(command ?? new RevokeAllSessionsCommand(), cancellationToken);
            return result.Match(Results.Ok, Results.BadRequest);
        })
        .WithName("RevokeAllSessions")
        .WithSummary("Revoke all sessions")
        //.RequirePermission(IdentityPermissionConstants.Sessions.Revoke)
        .WithDescription("Revoke all sessions for the currently authenticated user except the current one.");
    }
}
