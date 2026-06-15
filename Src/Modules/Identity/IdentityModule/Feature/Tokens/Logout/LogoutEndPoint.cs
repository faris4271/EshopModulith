using Carter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Identity.Contract.Feature.Tokens.RefreshToken;

namespace IdentityModule.Feature.Tokens.Logout
{
    internal class LogoutEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("api/identity/logout", async (HttpContext context) =>
            {
                // Clear the refresh token cookie
                context.Response.Cookies.Append("refreshToken", "", new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Path = "/",
                    Expires = DateTimeOffset.UtcNow.AddDays(-1)
                });
                context.Response.Cookies.Append("accessToken", "", new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Path = "/",
                    Expires = DateTimeOffset.UtcNow.AddDays(-1)
                });
                return Results.Ok(new { message = "Logged out successfully" });
            }).WithName("logout")
           .WithSummary("remove credinial")
           .WithDescription("logout")
           .Produces<RefreshTokenCommandResponse>(StatusCodes.Status200OK)
           .Produces(StatusCodes.Status401Unauthorized)
           .Produces(StatusCodes.Status400BadRequest)
           .Produces(StatusCodes.Status500InternalServerError).AllowAnonymous();
        }
    }
}
