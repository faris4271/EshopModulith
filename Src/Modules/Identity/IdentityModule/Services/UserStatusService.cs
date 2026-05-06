using IdentityModule.Domain;
using Microsoft.AspNetCore.Identity;
using Module.Identity.Contract.Services;
using SendGrid.Helpers.Errors.Model;
using Shared.Constants;
using Shared.Exeption;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityModule.Services
{
    public class UserStatusService
        (UserManager<AppUser> _userManager,
        ICurrentUser _currentUser
        ) : IUserStatusService
    {
        public async Task DeleteAsync(string userId)
        {
           ArgumentNullException.ThrowIfNullOrEmpty( userId );

            var user=await _userManager.FindByIdAsync( userId );

            if (user == null)
                throw new NotFoundException($"can not find userId:{userId}");

            user.IsActive= false;

           var result=  await  _userManager.UpdateAsync( user );

            if(!result.Succeeded)
            {
                throw new CustomException(result.Errors.FirstOrDefault().Description);
            }



        }

        public async Task ToggleStatusAsync(bool activateUser, string userId, CancellationToken cancellationToken)
        {
           var context=  await BuildToggleContextAsync(userId,activateUser,cancellationToken);

         await   ValidateTogglePermissionsAsync(context, cancellationToken);

         
            ApplyStatusChange(context);

           await  _userManager.UpdateAsync(context.TargetUser);




        }
        private static void ApplyStatusChange(ToggleStatusContext context)
        {
            if (context.ActivateUser)
            {
                context.TargetUser.Activate(context.ActorId.ToString());
            }
            else
            {
                context.TargetUser.Deactivate(context.ActorId.ToString(), "Status toggled by administrator");
            }
        }

        private async Task<ToggleStatusContext> BuildToggleContextAsync(
             string userId,
             bool activateUser,
             CancellationToken cancellationToken)
        {
            var targetUser=await _userManager.FindByIdAsync( userId );

            _ = targetUser ?? throw new NotFoundException("can not find this user");

            var actorId=_currentUser.GetUserId();

            if (actorId == Guid.Empty)
                throw new UnauthorizedException();

            var actore =await _userManager.FindByIdAsync(actorId.ToString());

            return new ToggleStatusContext
            (
               actorId,
               actore,
               targetUser,
               activateUser
            );
        }

        private async Task ValidateTogglePermissionsAsync(
        ToggleStatusContext context,
        CancellationToken cancellationToken)
        {
            if (!await _userManager.IsInRoleAsync(context.Actor, RoleConstants.Admin))
            {
               
                throw new CustomException("Only administrators can toggle user status.");
            }

            if (!context.ActivateUser && context.ActorId.ToString() == context.TargetUser.Id)
            {
               
                throw new CustomException("Users cannot deactivate themselves.");
            }

            if (await _userManager.IsInRoleAsync(context.TargetUser, RoleConstants.Admin))
            {
               
                throw new CustomException("Administrators cannot be deactivated.");
            }

            if (!context.ActivateUser)
            {
                await EnsureMinimumActiveAdminsAsync(context, cancellationToken);

            }
        }

        private async Task EnsureMinimumActiveAdminsAsync(
      ToggleStatusContext context,
      CancellationToken cancellationToken)
        {
            var active = await _userManager.GetUsersInRoleAsync(RoleConstants.Admin);

            if (!active.Contains(context.Actor))
            {
                throw new CustomException("user must be admin");
            }
        }
 private sealed record ToggleStatusContext(
Guid ActorId,
AppUser Actor,
AppUser TargetUser,
bool ActivateUser
);
    }


}
