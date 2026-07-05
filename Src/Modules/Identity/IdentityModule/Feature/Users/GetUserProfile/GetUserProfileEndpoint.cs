
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Identity.Contract.Feature.Users.GetUserProfile;
using SendGrid.Helpers.Errors.Model;
using Shared.Contract.Identity;
using Shared.Contract.ResultPattern;
using System.Security.Claims;

namespace IdentityModule.Feature.Users.GetUserProfile;

public class GetUserProfileEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/identity/profile", async (ClaimsPrincipal user, ISender sender, CancellationToken cancellationToken) =>
        {
            if (user.GetUserId() is not { } userId || string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedException();
            }

            var result = await sender.Send(new GetCurrentUserProfileQuery(userId), cancellationToken);

            return result.Match(Results.Ok, Results.BadRequest);

        })
        .WithName("GetCurrentUserProfile")
        .WithSummary("Get current user profile")
        .WithDescription("Retrieve the authenticated user's profile from the access token.")
        .RequireAuthorization();
    }
}
