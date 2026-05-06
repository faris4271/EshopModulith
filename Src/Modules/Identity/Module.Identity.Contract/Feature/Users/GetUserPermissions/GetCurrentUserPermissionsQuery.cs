
using Shared.Contract.CQRS;

namespace Module.Identity.Contract.Feature.Users.GetUserPermissions;

public sealed record GetCurrentUserPermissionsQuery(string UserId) : IQuery<List<string>?>;

