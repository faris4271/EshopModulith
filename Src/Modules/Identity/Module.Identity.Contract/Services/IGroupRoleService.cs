using System;
using System.Collections.Generic;
using System.Text;

namespace Module.Identity.Contract.Services
{
    public interface IGroupRoleService
    {
        Task<IReadOnlyList<string>> GetUserGroupRolesAsync(string userId, CancellationToken ct = default);
    }
}
