using IdentityModule.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Identity;
using Shared.Persistence;
using Shared.Web.Origin;

namespace IdentityModule.Data;

internal sealed class IdentityDbInitializer(
    ILogger<IdentityDbInitializer> logger,
    IdentityDbContext context,
    RoleManager<Role> roleManager,
    UserManager<AppUser> userManager,
    TimeProvider timeProvider,

    IOptions<OriginOptions> originSettings) : IDbInitializer
{
    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        if ((await context.Database.GetPendingMigrationsAsync(cancellationToken).ConfigureAwait(false)).Any())
        {
            await context.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation(" applied database migrations for identity module");
        }
    }

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        await SeedRolesAsync(cancellationToken);
        await SeedSystemGroupsAsync(cancellationToken);

    }

    private async Task SeedRolesAsync(CancellationToken cancellationToken = default)
    {
        foreach (string roleName in RoleConstants.DefaultRoles)
        {
            if (await roleManager.Roles.SingleOrDefaultAsync(r => r.Name == roleName, cancellationToken)
                is not Role role)
            {
                // create role
                role = new Role(roleName, $"{roleName} Role for Tenant");
                await roleManager.CreateAsync(role);
            }

            // Assign permissions
            if (roleName == RoleConstants.Basic)
            {
                await AssignPermissionsToRoleAsync(context, PermissionConstants.Basic, role, cancellationToken);
            }
            else if (roleName == RoleConstants.Admin)
            {
                await AssignPermissionsToRoleAsync(context, PermissionConstants.Admin, role, cancellationToken);



                await AssignPermissionsToRoleAsync(context, PermissionConstants.Root, role, cancellationToken);

            }
        }
    }

    private async Task AssignPermissionsToRoleAsync(IdentityDbContext dbContext, IReadOnlyList<AppPermission> permissions, Role role, CancellationToken cancellationToken = default)
    {
        var currentClaims = await roleManager.GetClaimsAsync(role);
        var newClaims = permissions
            .Where(permission => !currentClaims.Any(c => c.Type == ClaimConstants.Permission && c.Value == permission.Name))
            .Select(permission => new RoleClaim
            {
                RoleId = role.Id,
                ClaimType = ClaimConstants.Permission,
                ClaimValue = permission.Name,
                CreatedBy = "application",
                CreatedOn = timeProvider.GetUtcNow()
            })
            .ToList();

        foreach (var claim in newClaims)
        {
            //logger.LogInformation("Seeding {Role} Permission '{Permission}' for '{TenantId}' Tenant.", role.Name, claim.ClaimValue);
            await dbContext.RoleClaims.AddAsync(claim, cancellationToken);
        }

        // Save changes to the database context
        if (newClaims.Count != 0)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }

    }

    private async Task SeedSystemGroupsAsync(CancellationToken cancellationToken = default)
    {


        // Seed "All Users" default group - all new users are automatically added to this group
        const string allUsersGroupName = "All Users";
        var allUsersGroup = await context.Groups
            .FirstOrDefaultAsync(g => g.Name == allUsersGroupName && g.IsSystemGroup, cancellationToken);

        if (allUsersGroup is null)
        {
            allUsersGroup = Group.Create(
                name: allUsersGroupName,
                description: "Default group for all users. New users are automatically added to this group.",
                isDefault: true,
                isSystemGroup: true,
                createdBy: "System");

            await context.Groups.AddAsync(allUsersGroup, cancellationToken);
            //logger.LogInformation("Seeding '{GroupName}' system group for '{TenantId}' Tenant.", allUsersGroupName);
        }

        // Seed "Administrators" group with Admin role
        const string administratorsGroupName = "Administrators";
        var administratorsGroup = await context.Groups
            .FirstOrDefaultAsync(g => g.Name == administratorsGroupName && g.IsSystemGroup, cancellationToken);

        if (administratorsGroup is null)
        {
            administratorsGroup = Group.Create(
                name: administratorsGroupName,
                description: "System group for administrators with full administrative privileges.",
                isDefault: false,
                isSystemGroup: true,
                createdBy: "System");

            await context.Groups.AddAsync(administratorsGroup, cancellationToken);
            //logger.LogInformation("Seeding '{GroupName}' system group for '{TenantId}' Tenant.", administratorsGroupName);
        }

        await context.SaveChangesAsync(cancellationToken);

        // Assign Admin role to Administrators group
        var adminRole = await roleManager.FindByNameAsync(RoleConstants.Admin);
        if (adminRole is not null)
        {
            var existingGroupRole = await context.GroupRoles
                .FirstOrDefaultAsync(gr => gr.GroupId == administratorsGroup.Id && gr.RoleId == adminRole.Id, cancellationToken);

            if (existingGroupRole is null)
            {
                context.GroupRoles.Add(GroupRole.Create(administratorsGroup.Id, adminRole.Id));

                await context.SaveChangesAsync(cancellationToken);
                //logger.LogInformation("Assigned Admin role to '{GroupName}' group for '{TenantId}' Tenant.", administratorsGroupName);
            }
        }
    }


}

