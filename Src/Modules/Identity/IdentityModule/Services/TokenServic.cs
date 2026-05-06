using IdentityModule.Authorization.Jwt;
using MassTransit.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Module.Identity.Contract.Dtos;
using Module.Identity.Contract.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityModule.Services
{
    public class TokenServic : ITokenServic
    {
        private readonly JwtOptions _jwtOptions;

        private readonly IdentityMetrics _metrics;

        private readonly ILogger<TokenServic> _logger;
        public TokenServic(IOptions<JwtOptions> options,
            IdentityMetrics metrics, ILogger<TokenServic> logger)
        {
            _jwtOptions = options.Value;
            _metrics = metrics;
            _logger = logger;
        }
        public Task<TokenResponse> IssueAsync(string subject, IEnumerable<Claim> claims,  CancellationToken ct = default)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SigningKey));

            var cred=new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var AccessTokenExpirationMinutes=DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenMinutes);
            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: AccessTokenExpirationMinutes,
                signingCredentials: cred
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            var refreshToken=Convert.ToBase64String(Guid.NewGuid().ToByteArray());

            var refreshTokenExpirationDays=DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenDays);

            var email=claims.FirstOrDefault(x=>x.Type==ClaimTypes.Email)?.Value;

            _logger.LogInformation("Token generated for {Email}", email);

            _metrics.TokenGenerated(email);

            var response=new TokenResponse
            (
                accessToken,
                refreshToken,
                AccessTokenExpirationMinutes,
                refreshTokenExpirationDays
            );

            return Task.FromResult(response);

        }
    }
}
