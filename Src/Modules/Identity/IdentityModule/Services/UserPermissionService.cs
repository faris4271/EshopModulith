using Module.Identity.Contract.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityModule.Services
{
    internal class UserPermissionService : IUserPermissionService
    {
        public Task<List<string>?> GetPermissionsAsync(string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasPermissionAsync(string userId, string permission, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task InvalidatePermissionCacheAsync(string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
