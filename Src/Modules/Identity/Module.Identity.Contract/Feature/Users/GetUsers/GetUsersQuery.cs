using Module.Identity.Contract.Dtos;
using Shared.Contract.CQRS;

namespace Module.Identity.Contract.Feature.Users.GetUsers;

public sealed record GetUsersQuery : IQuery<List<UserDto>>;

