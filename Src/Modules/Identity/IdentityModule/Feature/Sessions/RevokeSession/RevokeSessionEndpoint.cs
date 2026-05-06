using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Module.Identity.Contract.Feature.Sessions.RevokeSession;
using Shared.Contract.ResultPattern;

namespace IdentityModule.Feature.Sessions.RevokeSession;

public  class RevokeSessionEndpoint:ICarterModule
{

  public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/sessions/{sessionId:guid}", Handler)
        .WithName("RevokeSession")
        .WithSummary("Revoke a session")
        //.RequirePermission(IdentityPermissionConstants.Sessions.Revoke)
        .WithDescription("Revoke a specific session for the currently authenticated user.");
    }
    private static async Task<IResult> Handler(
        Guid sessionId,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new RevokeSessionCommand(sessionId), cancellationToken);
        return result.Match(
            Results.Ok,
            Results.NotFound
        );
    }

  
}
