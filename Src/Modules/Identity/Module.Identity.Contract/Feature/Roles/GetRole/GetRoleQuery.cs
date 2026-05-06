using Module.Identity.Contract.Dtos;
using Shared.Contract.CQRS;

namespace Module.Identity.Contract.Feature.Roles.GetRole;

public sealed record GetRoleQuery(string Id) : IQuery<RoleDto?>;

