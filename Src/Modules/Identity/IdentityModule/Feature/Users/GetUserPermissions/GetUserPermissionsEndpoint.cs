
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Identity.Contract.Feature.Users.GetUserPermissions;
using SendGrid.Helpers.Errors.Model;
using Shared.Contract.Identity;
using Shared.Contract.ResultPattern;
using System.Net;
using System.Security.Claims;

namespace IdentityModule.Feature.Users.GetUserPermissions;

public class GetUserPermissionsEndpoint : ICarterModule
{



    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/permissions", async (ClaimsPrincipal user, ISender sender, CancellationToken cancellationToken) =>
        {
            if (user.GetUserId() is not { } userId || string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedException();
            }

            var result= await sender.Send(new GetCurrentUserPermissionsQuery(userId), cancellationToken);

          return  result.Match(Results.Ok, Results.BadRequest);
        })
         .WithName("GetCurrentUserPermissions")
        .WithSummary("Get current user permissions")
        .WithDescription("Retrieve permissions for the authenticated user.");
        //.RequirePermission(IdentityPermissionConstants.Users.View); ;
    }
}
