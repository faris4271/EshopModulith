
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Module.Identity.Contract.Feature.Users.ResetPassword;
using Shared.Contract.ResultPattern;
using System.Net;

namespace IdentityModule.Feature.Users.ResetPassword;

public class ResetPasswordEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
         app.MapPost("/reset-password",
         async ([FromBody] ResetPasswordCommand command,
         ISender sender,
         CancellationToken cancellationToken) =>
         {
             var result = await sender.Send(command, cancellationToken);
             return result.Match(Results.Ok, Results.BadRequest);
         })
     .WithName("ResetPassword")
     .WithSummary("Reset password")
     .WithDescription("Reset the user's password using the provided verification token.")
     .AllowAnonymous();
    }
}
