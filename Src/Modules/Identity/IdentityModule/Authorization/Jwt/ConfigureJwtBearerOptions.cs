using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SendGrid.Helpers.Errors.Model;
using Shared.DDD;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace IdentityModule.Authorization.Jwt
{
    internal class ConfigureJwtBearerOptions : IConfigureNamedOptions<JwtBearerOptions>
    {
        private readonly JwtOptions _options;
        public ConfigureJwtBearerOptions(IOptions<JwtOptions> options)
        {
            _options = options.Value;
        }
        public void Configure(string? name, JwtBearerOptions options)
        {
            ArgumentNullException.ThrowIfNull(options, nameof(options));

            if (name!=JwtBearerDefaults.AuthenticationScheme)
                return;

            byte[] key=Encoding.UTF8.GetBytes(_options.SigningKey);

            options.SaveToken = false;
            options.RequireHttpsMetadata = false;


            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidIssuer = _options.Issuer,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidAudience = _options.Audience,
                ValidateAudience = true,
                RoleClaimType = ClaimTypes.Role,
                ClockSkew = TimeSpan.FromMinutes(2)
            };


            options.Events = new JwtBearerEvents
            {
                OnChallenge = context =>
                {
                    context.HandleResponse();
                    if (!context.Response.HasStarted)
                    {
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        var result = System.Text.Json.JsonSerializer.Serialize(new { error = "Unauthorized" });
                        return context.Response.WriteAsync(result);
                    }
                    return Task.CompletedTask;
                },
                OnForbidden = _ => throw new ForbiddenException(),
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];
                    if (!string.IsNullOrEmpty(accessToken) &&
                        context.HttpContext.Request.Path.StartsWithSegments("/notifications", StringComparison.OrdinalIgnoreCase))
                    {
                        // Read the token out of the query string
                        context.Token = accessToken;
                    }
                    return Task.CompletedTask;
                }
            };

        }

        public void Configure(JwtBearerOptions options)
        {

        }
    }
}
