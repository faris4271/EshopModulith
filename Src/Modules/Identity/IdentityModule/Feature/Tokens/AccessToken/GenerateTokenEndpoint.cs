using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Module.Identity.Contract.Dtos;
using Module.Identity.Contract.Feature.Tokens.TokenGeneration;

namespace IdentityModule.Feature.Tokens.AccessToken;

public class GenerateTokenEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/identity/token", async (HttpContext context, [FromBody] GenerateTokenCommand command, [FromServices] ISender sender) =>
        {

            var result = await sender.Send(command);

            var refreshCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            var accessCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            context.Response.Cookies.Append("refreshToken", result.IsSuccess ? result.Value.RefreshToken : string.Empty, refreshCookieOptions);
            context.Response.Cookies.Append("accessToken", result.IsSuccess ? result.Value.AccessToken : string.Empty, accessCookieOptions);

            return result.Match(
                () => Results.Ok(new { accessToken = result.Value.AccessToken }),
                err => Results.BadRequest(err)
            );
        }).WithName("IssueJwtTokens")
            .WithSummary("Issue JWT access and refresh tokens")
            .WithDescription("Submit credentials to receive a JWT access token and a refresh token. Provide the 'tenant' header to select the tenant context (defaults to 'root').")
            .Produces<TokenResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .AllowAnonymous();
    }
}
