using IdentityModule.Data;
using IdentityModule.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Module.Identity.Contract.Dtos;
using Module.Identity.Contract.Services;
using SendGrid.Helpers.Errors.Model;

namespace IdentityModule.Services
{
    internal class UserRoleService : IUserRoleService
    {
        private readonly UserManager<AppUser> _userManager;

        private readonly RoleManager<AppRole> _roleManager;

        private readonly IdentityDbContext _identityDb;

        public UserRoleService(UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager, IdentityDbContext identityDb)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _identityDb = identityDb;
        }

        public async Task<string> AssignRolesAsync(string userId, List<UserRoleDto> userRoles, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrEmpty(userId, nameof(userId));
            ArgumentNullException.ThrowIfNull(userRoles, nameof(userRoles));

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new NotFoundException("can not find user with the given id");
            }
            var assignedRoles = await ProcessRoleAssignmentsAsync(user, userRoles);

            await RaiseRolesAssignedEventAsync(user, assignedRoles, cancellationToken);

            return "Roles assigned successfully";

        }

        public async Task<List<UserRoleDto>> GetUserRolesAsync(string userId, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrEmpty(userId, nameof(userId));

            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new NotFoundException("can not find user with the given id");

            var roles = await _roleManager.Roles.ToListAsync();
            var enrolledRoles = await _userManager.GetRolesAsync(user);

            var userRoles = new List<UserRoleDto>();

            foreach (var role in roles)
            {
                var userRole = new UserRoleDto
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    Description = role.Description,
                    Enabled = enrolledRoles.Contains(role.Name)
                };
                userRoles.Add(userRole);
            }
            return userRoles;

        }

        private async Task<List<string>> ProcessRoleAssignmentsAsync(AppUser user, List<UserRoleDto> userRoles)
        {
            var AssignRoles = new List<string>();

            var currentRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in userRoles)
            {
                if (!await _roleManager.RoleExistsAsync(role.RoleName))
                {
                    continue;
                }

                if (role.Enabled && !currentRoles.Contains(role.RoleName))
                {
                    AssignRoles.Add(role.RoleName);
                }
                else if (!role.Enabled && currentRoles.Contains(role.RoleName))
                {
                    await _userManager.RemoveFromRoleAsync(user, role.RoleName);
                }
                AssignRoles.Add(role.RoleName);
            }
            return AssignRoles;

        }

        private async Task RaiseRolesAssignedEventAsync(AppUser user, List<string> assignedRoles, CancellationToken cancellationToken)
        {
            if (assignedRoles.Count == 0)
            {
                return;
            }

            user.RecordRolesAssigned(assignedRoles);

            await _identityDb.SaveChangesAsync(cancellationToken);


        }
    }
}
