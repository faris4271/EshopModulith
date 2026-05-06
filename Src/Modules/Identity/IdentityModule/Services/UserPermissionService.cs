using IdentityModule.Data;
using IdentityModule.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Module.Identity.Contract.Services;
using Shared.Caching;
using Shared.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityModule.Services
{
    internal class UserPermissionService(
    UserManager<AppUser> _userManager,
    RoleManager<Role> _roleManager,
    IdentityDbContext _db,
    ICacheService cache) : IUserPermissionService
    {
        public async Task<List<string>?> GetPermissionsAsync(string userId, CancellationToken cancellationToken)
        {
            var permissions = await cache.GetOrSetAsync(GetPermissionCacheKey(userId),async () =>
            {
                ArgumentNullException.ThrowIfNull(userId, nameof(userId));

                var user = await _userManager.FindByIdAsync(userId);

                _=user ?? throw new UnauthorizedAccessException($"User with ID '{userId}' not found.");

                var userRoles = await _userManager.GetRolesAsync(user);

                var permissions = new List<string>();

                foreach (var role in await _roleManager.Roles.
                Where(r => userRoles.Contains(r.Name)).ToListAsync(cancellationToken))
                {
                    permissions.AddRange(await _db.RoleClaims.
                        Where(x => x.RoleId == role.Id &&
                        x.ClaimType == ClaimConstants.Permission).
                        Select(x=>x.ClaimValue!).ToListAsync(cancellationToken));

                }
                return permissions.Distinct().ToList();




            },cancellationToken: cancellationToken);

            return permissions;

        }

        public async Task<bool> HasPermissionAsync(string userId, string permission, CancellationToken cancellationToken = default)
        {
            var permissionRetrived = await GetPermissionsAsync(userId, cancellationToken);

            return permissionRetrived?.Contains(permission) ?? false;
        }

        public Task InvalidatePermissionCacheAsync(string userId, CancellationToken cancellationToken)
        {
            return cache.RemoveItemAsync(GetPermissionCacheKey(userId), cancellationToken);
        }

        public static string GetPermissionCacheKey(string userId)
        {
            return $"perm:{userId}";
        }
    }
}
