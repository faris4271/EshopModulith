using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Module.Identity.Contract.Feature.Users.ForgotPassword;
using Shared.Contract.ResultPattern;
using Shared.DDD;

namespace IdentityModule.Feature.Users.ForgotPassword;

public  class ForgotPasswordEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/forgot-password", async (ISender sender, ForgotPasswordCommand command) =>
        {
            var result = await sender.Send(command);

           return result.Match(Results.NoContent, Results.BadRequest);
        });
    }
}
