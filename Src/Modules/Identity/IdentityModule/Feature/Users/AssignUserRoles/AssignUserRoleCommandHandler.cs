using Module.Identity.Contract.Feature.Users.AssignUserRoles;
using Module.Identity.Contract.Services;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityModule.Feature.Users.AssignUserRoles
{
    internal class AssignUserRoleCommandHandler(IUserService _userService) : ICommandHandler<AssignUserRolesCommand, string>
    {
        public async Task<Result<string>> Handle(AssignUserRolesCommand request, CancellationToken cancellationToken)
        {
            var result = await _userService.AssignRolesAsync(request.UserId, request.UserRoles, cancellationToken);
            return Result.Success(result);
        }
    }
}
