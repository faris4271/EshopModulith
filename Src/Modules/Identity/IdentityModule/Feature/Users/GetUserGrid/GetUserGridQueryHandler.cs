using IdentityModule.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Module.Identity.Contract.Dtos;
using Module.Identity.Contract.Feature.Users.GetUserGrid;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using Shared.Web.SmartTable;

namespace IdentityModule.Feature.Users.GetUserGrid
{
    internal class GetUserGridQueryHandler(UserManager<AppUser> userManager) : IQueryHandler<GetUserGridQuery, SmartTableResult<GetUserGridDto>>
    {
        public async Task<Result<SmartTableResult<GetUserGridDto>>> Handle(GetUserGridQuery request, CancellationToken cancellationToken)
        {
            var user = userManager.Users.AsNoTracking().AsQueryable();



            var result = user.ToSmartTableResult(
             request.SmartTableParam,
             x => new GetUserGridDto
             (
                 x.Id,
                 x.UserName,
                 x.Email,
                 x.CreatedOn,
                 x.Roles.Select(r => r.Role.Name).ToList(),
                 x.GroupUsers.Select(g => g.Group.Name).ToList()

            ));

            return Result.Success(result);

        }

    }
}

