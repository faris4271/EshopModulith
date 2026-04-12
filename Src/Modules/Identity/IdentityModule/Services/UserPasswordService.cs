using IdentityModule.Data;
using IdentityModule.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Module.Identity.Contract.Services;
using Shared.Exeption;
using Shared.Jobs.Services;
using Shared.Mailing;
using Shared.Mailing.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace IdentityModule.Services
{
    internal class UserPasswordService : IUserPasswordService
    {
        private readonly UserManager<AppUser> _userManager;

        private readonly IdentityDbContext _identityDb;

        private readonly IMailService _emailService;

        private readonly IJobService _jobService;

        public UserPasswordService(UserManager<AppUser> userManager, IdentityDbContext identityDb, IMailService emailService, IJobService jobService)
        {
            _userManager = userManager;
            _identityDb = identityDb;
            _emailService = emailService;
            _jobService = jobService;
        }

        public async Task ChangePasswordAsync(string password, string newPassword, string confirmNewPassword, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            _ = user ?? throw new ArgumentNullException(nameof(user));

            var result = await _userManager.ChangePasswordAsync(user, password, newPassword);

            if(!result.Succeeded)
            {
                var error = result.Errors.Select(x => x.Description).ToList();

                throw new CustomException(string.Join("failed to change password", error));
            }
                await _userManager.UpdateAsync(user);

              user.RecordPasswordChanged(false);

            await _identityDb.SaveChangesAsync();

        }

        public async Task ForgotPasswordAsync(string email, string origin, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(email, nameof(email));
            ArgumentNullException.ThrowIfNullOrEmpty(origin, nameof(origin));

            var user =await _userManager.FindByEmailAsync(email);

            if (user == null) 
                throw new ArgumentNullException(nameof(user));

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            token= WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var resetPasswordUri = $"{origin}/reset-password?token={token}&email={email}";
            var mailRequest = new MailRequest(
                new Collection<string> { user.Email },
                "Reset Password",
                $"Please reset your password using the following link: {resetPasswordUri}");

            _jobService.Enqueue(()=>_emailService.SendAsync(mailRequest,cancellationToken));


        }

        public async Task ResetPasswordAsync(string email, string password, string token, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(email, nameof(email));
            ArgumentNullException.ThrowIfNullOrEmpty(password, nameof(password));
            ArgumentNullException.ThrowIfNullOrEmpty(token, nameof(token));

            var user = await _userManager.FindByEmailAsync(email);

            if(user == null)
                throw new ArgumentNullException(nameof(user));

             token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            var result =await _userManager.ResetPasswordAsync(user, token, password);

            if(!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    throw new CustomException(error.Description);
                }
            }
            user.RecordPasswordChanged(true);
            await _userManager.UpdateAsync(user);
        }
    }
}
