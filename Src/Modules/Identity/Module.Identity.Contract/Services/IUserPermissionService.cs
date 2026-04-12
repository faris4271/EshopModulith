using System;
using System.Collections.Generic;
using System.Text;

namespace Module.Identity.Contract.Services
{
    public interface IUserPermissionService
    {
        Task<List<string>?> GetPermissionsAsync(string userId, CancellationToken cancellationToken);

        /// <summary>
        /// Checks if a user has a specific permission.
        /// </summary>
        Task<bool> HasPermissionAsync(string userId, string permission, CancellationToken cancellationToken = default);

        /// <summary>
        /// Invalidates the permission cache for a user.
        /// </summary>
        Task InvalidatePermissionCacheAsync(string userId, CancellationToken cancellationToken);
    }
}
