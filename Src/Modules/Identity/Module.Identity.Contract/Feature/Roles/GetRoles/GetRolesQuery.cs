using Module.Identity.Contract.Dtos;
using Shared.Contract.CQRS;

namespace Module.Identity.Contract.Feature.Roles.GetRoles;

public sealed record GetRolesQuery : IQuery<IEnumerable<RoleDto>>;

