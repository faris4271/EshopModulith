using Module.Identity.Contract.Dtos;
using Shared.Contract.Context;

namespace Module.Identity.Contract.Services
{
    public interface ICurrentUserService : ICurrentUser, ICurrentUserInitializer
    {
         Task<UserDto> GetCurrentUser();
    }
}
