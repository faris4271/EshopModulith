using IdentityModule.Data;
using Module.Identity.Contract.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityModule.Services
{
    internal class GroupRoleService(IdentityDbContext _dbContext) : IGroupRoleService
    {
        public async Task<IReadOnlyList<string>> GetUserGroupRolesAsync(string userId, CancellationToken ct = default)
        {
            var userGroup=_dbContext.UserGroups.
                Where(x=>x.UserId == userId).Select(x=>x.GroupId).ToList();

            var groupRole=_dbContext.GroupRoles
                .Where(x=>userGroup.Contains(x.GroupId)).Select(x=>x.Role!.Name!).Distinct().ToList();
            return  groupRole;

        }
    }
}
