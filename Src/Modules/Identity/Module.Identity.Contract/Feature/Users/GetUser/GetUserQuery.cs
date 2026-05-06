
using Module.Identity.Contract.Dtos;
using Shared.Contract.CQRS;

namespace Module.Identity.Contract.Feature.Users.GetUser;

public sealed record GetUserQuery(string Id) : IQuery<UserDto>;

