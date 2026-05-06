using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Module.Identity.Contract.Feature.Sessions.AdminRevokeSession;
using Shared.Contract.ResultPattern;
using System.Net;

namespace IdentityModule.Feature.Sessions.AdminRevokeSession;

public  class AdminRevokeSessionEndpoint:ICarterModule
{
      public void AddRoutes(IEndpointRouteBuilder app)
    {
         app.MapDelete("/users/{userId:guid}/sessions/{sessionId:guid}", Handler)
        .WithName("AdminRevokeSession")
        .WithSummary("Revoke a user's session (Admin)")
        //.RequirePermission(IdentityPermissionConstants.Sessions.RevokeAll)
        .WithDescription("Revoke a specific session for a user. Requires admin permission.");
    }

    private static async Task<IResult> Handler(
        Guid userId,
        Guid sessionId,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new AdminRevokeSessionCommand(userId, sessionId), cancellationToken);
        return result.Match(Results.Ok, Results.BadRequest);
    }

  
}
