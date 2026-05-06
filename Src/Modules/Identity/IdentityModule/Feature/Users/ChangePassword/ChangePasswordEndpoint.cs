using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Module.Identity.Contract.Feature.Users.ChangePassword;
using Shared.Contract.ResultPattern;


namespace IdentityModule.Feature.Users.ChangePassword;

public  class ChangePasswordEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/change-password", async (
             [FromBody] ChangePasswordCommand command,
              ISender sender,
               CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(command, cancellationToken);
            return result.Match(Results.Ok, Results.BadRequest);
        })
           .WithName("ChangePassword")
            .WithSummary("Change password")
            .WithDescription("Change the current user's password.")
            .RequireAuthorization();
    }
}
