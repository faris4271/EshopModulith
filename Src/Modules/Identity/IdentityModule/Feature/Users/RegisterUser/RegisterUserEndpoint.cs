using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Identity.Contract.Feature.Users.RegisterUser;
using Shared.Contract.ResultPattern;

namespace IdentityModule.Feature.Users.RegisterUser;

public class RegisterUserEndpointL : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/identity/register", async (RegisterUserCommand command,
           HttpContext context,
           ISender sender,
           CancellationToken cancellationToken) =>
        {
            var origin = $"{context.Request.Scheme}://{context.Request.Host.Value}{context.Request.PathBase.Value}";
            command.Origin = origin;

            var result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, Results.BadRequest);
        })
       .WithName("RegisterUser")
       .WithSummary("Register user").AllowAnonymous()
       //.RequirePermission(IdentityPermissionConstants.Users.Create)
       .WithDescription("Create a new user account.");
    }
}
