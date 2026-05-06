using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Identity.Contract.Feature.Sessions.GetMySessions;
using Shared.Contract.ResultPattern;

namespace IdentityModule.Feature.Sessions.GetMySessions;

public  class GetMySessionsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/sessions/me", async (CancellationToken cancellationToken, ISender sender) =>
        {
          var result=await  sender.Send(new GetMySessionsQuery(), cancellationToken);

         return   result.Match(Results.Ok,Results.NotFound);


        })
       .WithName("GetMySessions")
       .WithSummary("Get current user's sessions")
       //.RequirePermission(IdentityPermissionConstants.Sessions.View)
       .WithDescription("Retrieve all active sessions for the currently authenticated user.");
    }
}
