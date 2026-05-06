

using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Module.Identity.Contract.Feature.Users.UpdateUser;
using SendGrid.Helpers.Errors.Model;
using Shared.Contract.Identity;
using System.Security.Claims;

namespace IdentityModule.Feature.Users.UpdateUser;

public class UpdateUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
         app.MapPut("/profile", async ([FromBody] UpdateUserCommand request, ClaimsPrincipal user, ISender sender, CancellationToken cancellationToken) =>
        {
            if (user.GetUserId() is not { } userId || string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedException();
            }

            request.Id = userId;

            var result= await sender.Send(request, cancellationToken);

            return result.Match(Results.NoContent, Results.BadRequest);
        })
       .WithName("UpdateUserProfile")
       .WithSummary("Update user profile")
       //.RequirePermission(IdentityPermissionConstants.Users.Update)
       .WithDescription("Update profile details for the authenticated user.");
    }
}
