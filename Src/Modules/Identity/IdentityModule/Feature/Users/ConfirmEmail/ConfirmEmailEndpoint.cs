using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Identity.Contract.Feature.Users.ConfirmEmail;
using Shared.Contract.ResultPattern;
using System.Net;

namespace IdentityModule.Feature.Users.ConfirmEmail;

public class ConfirmEmailEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/confirm-email", async (string userId, string code, string tenant, ISender sender, CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new ConfirmEmailCommand(userId, code), cancellationToken);

            result.Match(Results.Ok, Results.BadRequest);

        })
        .WithName("ConfirmEmail")
        .WithSummary("Confirm user email")
        .WithDescription("Confirm a user's email address.")
        .AllowAnonymous();
    }
}
