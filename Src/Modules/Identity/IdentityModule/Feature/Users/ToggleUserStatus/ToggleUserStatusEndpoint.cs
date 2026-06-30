using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Module.Identity.Contract.Feature.Users.ToggleUserStatus;

namespace IdentityModule.Feature.Users.ToggleUserStatus;

public class ToggleUserStatusEndpoint : ICarterModule
{

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch("/users/{id:guid}", Handler)
     .WithName("ToggleUserStatus")
     .WithSummary("Toggle user status")
     //.RequirePermission(IdentityPermissionConstants.Users.Update)
     .WithDescription("Activate or deactivate a user account.");
    }

    private static async Task<Results<NoContent, BadRequest>> Handler(
    string id,
    [FromBody] ToggleUserStatusCommand command,
    ISender sender,
    CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(command.UserId))
        {
            command.UserId = id;
        }

        if (!string.Equals(id, command.UserId, StringComparison.Ordinal))
        {
            return TypedResults.BadRequest();
        }

        var result = await sender.Send(command, cancellationToken);

        return TypedResults.NoContent();
    }
}
