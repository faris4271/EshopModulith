using System;
using System.Collections.Generic;
using System.Text;

namespace Module.Identity.Contract.Services
{
    public interface IUserPasswordService
    {
        /// <summary>
        /// Initiates the forgot password flow by sending a reset email.
        /// </summary>
        Task ForgotPasswordAsync(string email, string origin, CancellationToken cancellationToken);

        /// <summary>
        /// Resets a user's password using a token.
        /// </summary>
        Task ResetPasswordAsync(string email, string password, string token, CancellationToken cancellationToken);

        /// <summary>
        /// Changes the current user's password.
        /// </summary>
        Task ChangePasswordAsync(string password, string newPassword, string confirmNewPassword, string userId);
    }
}
