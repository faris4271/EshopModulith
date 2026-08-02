using Module.Identity.Contract.Dtos;
using Shared.Contract.CQRS;

namespace Module.Identity.Contract.Feature.Users.GetUserAddrees
{
    public record GetUserAddreesQuery(Guid userAddressId) : IQuery<GetUserAddressDto>;

}
