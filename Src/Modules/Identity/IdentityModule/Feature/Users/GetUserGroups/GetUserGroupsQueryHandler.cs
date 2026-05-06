using IdentityModule.Data;
using Microsoft.EntityFrameworkCore;
using Module.Identity.Contract.Dtos;
using Module.Identity.Contract.Feature.Users.GetUserGroups;
using SendGrid.Helpers.Errors.Model;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace IdentityModule.Feature.Users.GetUserGroups;

public sealed class GetUserGroupsQueryHandler : IQueryHandler<GetUserGroupsQuery, IEnumerable<GroupDto>>
{
    private readonly IdentityDbContext _dbContext;

    public GetUserGroupsQueryHandler(IdentityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<IEnumerable<GroupDto>>> Handle(GetUserGroupsQuery request, CancellationToken cancellationToken)
    {

        // Validate user exists
        var userExists = await _dbContext.Users
            .AnyAsync(u => u.Id == request.UserId, cancellationToken);

        if (!userExists)
        {
            return Result.Failure<IEnumerable<GroupDto>>(Error.NotFound("404", "Can found user"));
        }

        // Get user's groups
        var groupIds = await _dbContext.UserGroups
            .Where(ug => ug.UserId == request.UserId)
            .Select(ug => ug.GroupId)
            .ToListAsync(cancellationToken);

        if (groupIds.Count == 0)
        {
            return Result.Failure<IEnumerable<GroupDto>>(Error.NotFound("404", "Can found groupa"));
        }

        var groups = await _dbContext.Groups
            .Include(g => g.GroupRoles)
            .Where(g => groupIds.Contains(g.Id))
            .ToListAsync(cancellationToken);

        // Get member counts
        var memberCounts = await _dbContext.UserGroups
            .Where(ug => groupIds.Contains(ug.GroupId))
            .GroupBy(ug => ug.GroupId)
            .Select(g => new { GroupId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.GroupId, x => x.Count, cancellationToken);

        // Get role names
        var allRoleIds = groups
            .SelectMany(g => g.GroupRoles.Select(gr => gr.RoleId))
            .Distinct()
            .ToList();

        var roleNames = allRoleIds.Count > 0
            ? await _dbContext.Roles
                .Where(r => allRoleIds.Contains(r.Id))
                .ToDictionaryAsync(r => r.Id, r => r.Name!, cancellationToken)
            : new Dictionary<string, string>();

        var result= groups.Select(g => new GroupDto
        {
            Id = g.Id,
            Name = g.Name,
            Description = g.Description,
            IsDefault = g.IsDefault,
            IsSystemGroup = g.IsSystemGroup,
            MemberCount = memberCounts.GetValueOrDefault(g.Id, 0),
            RoleIds = g.GroupRoles.Select(gr => gr.RoleId).ToList().AsReadOnly(),
            RoleNames = g.GroupRoles
                .Select(gr => roleNames.GetValueOrDefault(gr.RoleId, gr.RoleId))
                .ToList()
                .AsReadOnly(),
            CreatedAt = g.CreatedAt
        });

        return Result.Success(result);
    }
}
