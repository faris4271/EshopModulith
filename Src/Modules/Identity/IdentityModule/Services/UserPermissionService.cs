using IdentityModule.Data;
using IdentityModule.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Module.Identity.Contract.Services;
using Shared.Caching;
using Shared.Identity;

namespace IdentityModule.Services
{
    internal class UserPermissionService(
        UserManager<AppUser> _userManager,
        RoleManager<AppRole> _roleManager,
        IdentityDbContext _db,
        HybridCache cache) : IUserPermissionService
    {

        private readonly record struct PermissionFactoryState(IdentityDbContext DbContext, string UserId);

        private static readonly HybridCacheEntryOptions PermissionEntryOptions = new()
        {
            Expiration = TimeSpan.FromHours(1),
            LocalCacheExpiration = TimeSpan.FromMinutes(2),
        };

        public async Task<List<string>> GetPermissionsAsync(string userId, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(userId, nameof(userId));

            var state = new PermissionFactoryState(_db, userId);
            var tags = new[] { CacheKeys.UserPermissions(userId), CacheKeys.Tags.Permissions };

            var permissions = await cache.GetOrCreateAsync(
                GetPermissionCacheKey(userId),
                state,
                LoadPermissionFromDbAsync,
                PermissionEntryOptions,
                tags,
                ct);

            return permissions ?? new List<string>();
        }

        private static async ValueTask<List<string>> LoadPermissionFromDbAsync(
            PermissionFactoryState state,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(state.UserId, nameof(state.UserId));


            var userExists = await state.DbContext.Users
                .AnyAsync(u => u.Id == state.UserId, cancellationToken);

            if (!userExists)
            {
                throw new UnauthorizedAccessException($"User with ID '{state.UserId}' not found.");
            }


            var permissions = await (
                from userRole in state.DbContext.UserRoles
                where userRole.UserId == state.UserId
                join roleClaim in state.DbContext.RoleClaims on userRole.RoleId equals roleClaim.RoleId
                where roleClaim.ClaimType == ClaimConstants.Permission && roleClaim.ClaimValue != null
                select roleClaim.ClaimValue!
            )
            .Distinct()
            .ToListAsync(cancellationToken);

            return permissions;
        }

        public async Task<bool> HasPermissionAsync(string userId, string permission, CancellationToken cancellationToken = default)
        {
            var permissions = await GetPermissionsAsync(userId, cancellationToken);
            return permissions.Contains(permission);
        }

        public ValueTask InvalidatePermissionCacheAsync(string userId, CancellationToken cancellationToken = default)
        {
            return cache.RemoveAsync(GetPermissionCacheKey(userId), cancellationToken);
        }

        public static string GetPermissionCacheKey(string userId)
        {
            return $"perm:{userId}";
        }
    }
}