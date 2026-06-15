using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Module.Identity.Contract.Feature.Tokens.RefreshToken;

namespace IdentityModule.Feature.Tokens.RefreshToken;

public class RefreshTokenEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/token/refresh", async (HttpContext context,
           [FromServices] ISender sender,
           CancellationToken ct) =>
           {

               var refreshToken = context.Request.Cookies["refreshToken"];
               if (refreshToken is null)
                   return Results.Unauthorized();

               var accessToken = context.Request.Cookies["accessToken"];
               if (accessToken is null)
                   return Results.Unauthorized();

               var command = new RefreshTokenCommand(accessToken, refreshToken);

               var result = await sender.Send(command, ct);

               if (result.IsSuccess)
               {
                   context.Response.Cookies.Append("refreshToken", "", new CookieOptions
                   {
                       HttpOnly = true,
                       Secure = true,
                       SameSite = SameSiteMode.None,
                       Path = "/",
                       Expires = DateTimeOffset.UtcNow.AddDays(-1)
                   });
                   context.Response.Cookies.Append("accessToken", "", new CookieOptions
                   {
                       HttpOnly = true,
                       Secure = true,
                       SameSite = SameSiteMode.None,
                       Path = "/",
                       Expires = DateTimeOffset.UtcNow.AddDays(-1)
                   });

                   var accessCookieOptions = new CookieOptions
                   {
                       HttpOnly = true,
                       Secure = true,
                       SameSite = SameSiteMode.None,
                       Expires = DateTime.UtcNow.AddDays(7)
                   };
                   var refreshCookieOptions = new CookieOptions
                   {
                       HttpOnly = true,
                       Secure = true,
                       SameSite = SameSiteMode.None,
                       Expires = DateTime.UtcNow.AddDays(7)
                   };
                   context.Response.Cookies.Append("accessToken", result.IsSuccess ? result.Value.Token : string.Empty, accessCookieOptions);
                   context.Response.Cookies.Append("refreshToken", result.IsSuccess ? result.Value.RefreshToken : string.Empty, refreshCookieOptions);
               }
               return result.Match(
                   onSuccess: () => Results.Ok(),
                   onFailure: (failedResult) => Results.Unauthorized()
               );
           })
           .WithName("RefreshJwtTokens")
           .WithSummary("Refresh JWT access and refresh tokens")
           .WithDescription("Use a valid (possibly expired) access token together with a valid refresh token to obtain a new access token and a rotated refresh token.")
           .Produces<RefreshTokenCommandResponse>(StatusCodes.Status200OK)
           .Produces(StatusCodes.Status401Unauthorized)
           .Produces(StatusCodes.Status400BadRequest)
           .Produces(StatusCodes.Status500InternalServerError)
           .AllowAnonymous();
    }
}

