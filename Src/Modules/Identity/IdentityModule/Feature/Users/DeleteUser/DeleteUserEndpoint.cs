using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Identity.Contract.Feature.Users.DeleteUser;

namespace IdentityModule.Feature.Users.DeleteUser;

public class DeleteUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/users/{id:guid}", async (string id, ISender mediator, CancellationToken cancellationToken) =>
       {
           var result = await mediator.Send(new DeleteUserCommand(id), cancellationToken);

           return result.Match(Results.NoContent, Results.BadRequest);

       })
    .WithName("DeleteUser").WithTags("Users")
    .WithSummary("Delete user")
    .WithDescription("Delete a user by unique identifier.");
    }
}
