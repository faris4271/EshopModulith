using IdentityModule.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Module.Identity.Contract.Services;
using SendGrid.Helpers.Errors.Model;
using Shared.Contract.Exeption;
using Shared.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IdentityModule.Services
{
    internal class IdentityService(
        UserManager<AppUser> _userManager,
        ILogger<IdentityService> _logger, IGroupRoleService _groupRoleService)
        : IIdentityService
    {
        public async Task StoreRefreshTokenAsync(string subject, string refreshToken, DateTime expiresAtUtc, CancellationToken ct = default)
        {
            var user = await _userManager.FindByIdAsync(subject);

            if (user == null)
                throw new CustomException("User not found");

            var hashedToken = HashToken(refreshToken);

            user.RefreshToken = hashedToken;

            user.RefreshTokenExpiryTime = expiresAtUtc;

            _logger.LogDebug(
       "Storing refresh token for user {UserId} in tenant {TenantId}. Token hash: {TokenHash}, Expires: {ExpiresAt}",
       subject, hashedToken[..Math.Min(8, hashedToken.Length)] + "...", expiresAtUtc);

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                _logger.LogError("Failed to persist refresh token for user {UserId}: {Errors}",
                    subject, string.Join(", ", result.Errors.Select(e => e.Description)));
                throw new UnauthorizedException("could not persist refresh token");
            }


        }

        public async Task<(string Subject, IEnumerable<Claim> Claims)?> ValidateCredentialsAsync(string email, string password, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(email, nameof(email));
            ArgumentNullException.ThrowIfNull(password, nameof(password));

            var user = await FindAndValidateUserByCredentialsAsync(email, password);
            ValidateUserStatus(user);
            var claims = await BuildUserClaimsAsync(user, ct);

            return (user.Id, claims);


        }

        private async Task<List<Claim>> BuildUserClaimsAsync(AppUser user, CancellationToken ct)
        {
            var basicClaims = new List<Claim>
            {
        new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new(ClaimTypes.NameIdentifier, user.Id),
        new(JwtRegisteredClaimNames.Sub, user.Id),
        new(ClaimTypes.Email, user.Email!),
        new(ClaimTypes.Name, user.FirstName ?? string.Empty),
        new(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty),
        new(ClaimConstants.Fullname, $"{user.FirstName} {user.LastName}"),
        new(ClaimTypes.Surname, user.LastName ?? string.Empty),
        new(ClaimConstants.ImageUrl, user.ImageUrl?.ToString() ?? string.Empty)
           };


            await AddRoleClaimsAsync(basicClaims, user, ct);

            return basicClaims;
        }


        private async Task AddRoleClaimsAsync(List<Claim> claims, AppUser user, CancellationToken ct)
        {
            var userroles = await _userManager.GetRolesAsync(user);

            var groupRole = await _groupRoleService.GetUserGroupRolesAsync(user.Id, ct);

            var allRoles = userroles.Union(groupRole).Distinct();

            claims.AddRange(allRoles.Select(x => new Claim(ClaimTypes.Role, x)));
        }

        private async Task<AppUser> FindAndValidateUserByCredentialsAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null && !await _userManager.CheckPasswordAsync(user, password))
                throw new UnauthorizedException("Invalid email or password");

            return user;
        }

        public async Task<(string Subject, IEnumerable<Claim> Claims)?> ValidateRefreshTokenAsync(string refreshToken, CancellationToken ct = default)
        {

            var user = await FindUserByRefreshTokenAsync(refreshToken, ct);

            ValidateRefreshTokenExpiry(user);
            ValidateUserStatus(user);


            var claims = await BuildUserClaimsAsync(user, ct);
            return (user.Id, claims);
        }
        private async Task<AppUser> FindUserByRefreshTokenAsync(string refreshToken, CancellationToken ct)
        {
            var hashedToken = HashToken(refreshToken);


            _logger.LogDebug(
                "Validating refresh token ",
                hashedToken[..Math.Min(8, hashedToken.Length)] + "...");

            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.RefreshToken == hashedToken, ct);
            if (user is null)
            {
                _logger.LogWarning("No user found with matching refresh token hash for tenant {TenantId}");
                throw new UnauthorizedException("refresh token is invalid or expired");
            }
            return user;
        }

        private void ValidateRefreshTokenExpiry(AppUser user)
        {
            if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                _logger.LogWarning(
                    "Refresh token expired for user {UserId}. Expired at: {ExpiryTime}, Current time: {CurrentTime}",
                    user.Id, user.RefreshTokenExpiryTime, DateTime.UtcNow);
                throw new UnauthorizedException("refresh token is invalid or expired");
            }
        }
        private void ValidateUserStatus(AppUser user)
        {
            if (!user.IsActive)
                throw new InvalidOperationException();
            if (user.EmailConfirmed)
                throw new InvalidOperationException();

        }

        private string HashToken(string refreshToken)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(refreshToken);
            var hash = System.Security.Cryptography.SHA256.HashData(bytes);
            return Convert.ToBase64String(hash);
        }

    }
}
