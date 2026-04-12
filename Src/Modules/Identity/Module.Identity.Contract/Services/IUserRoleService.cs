using Module.Identity.Contract.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Module.Identity.Contract.Services
{
    public interface IUserRoleService
    {
        Task<string> AssignRolesAsync(string userId, List<UserRoleDto> userRoles, CancellationToken cancellationToken);

    
        Task<List<UserRoleDto>> GetUserRolesAsync(string userId, CancellationToken cancellationToken);
    }
}
