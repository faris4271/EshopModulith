using IdentityModule.Data;
using IdentityModule.Domain;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Module.Identity.Contract.Events;
using Module.Identity.Contract.Services;
using Shared.Abstraction;
using Shared.Contract.Exeption;
using Shared.Identity;
using Shared.Mailing;
using Shared.Mailing.Services;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Security.Claims;
using System.Text;

namespace IdentityModule.Services
{
    public class UserRegistrationService(
        UserManager<AppUser> _userManager,
        IGenericeRepository<Group, IdentityDbContext> _gruopRepository,
        IGenericeRepository<UserGroup, IdentityDbContext> _userGroupRepository,
        IMailService _mailService,
        IPublishEndpoint _publish

        ) : IUserRegistrationService
    {


        public async Task<string> RegisterAsync(string firstName, string lastName, string email, string userName, string password, string confirmPassword, string phoneNumber, string origin, CancellationToken cancellationToken)
        {
            ValidatePasswordMatch(password, confirmPassword);

            var user = await CreateUserWithPasswordAsync(firstName, lastName, email, userName, password, phoneNumber);

            await AssignDefaultRoleAndGroupsAsync(user, "System", cancellationToken);

            await SendConfirmationEmailAsync(user, origin, cancellationToken);

            return user.Id;

        }

        private async Task SendConfirmationEmailAsync(AppUser user, string origin, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(user.Email))
                return;

            var emailVerficationUrl = await GetEmailVerificationUriAsync(user, origin);

            var emailBody = BuildConfirmationEmailHtml(user.FirstName ?? user.UserName ?? "user", emailVerficationUrl);


            var mailRequest = new MailRequest(new Collection<string> { user.Email }, "confirm email", emailBody);

            await _mailService.SendAsync(mailRequest, cancellationToken);


        }
        private async Task<string> GetEmailVerificationUriAsync(AppUser user, string origin)
        {


            string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            const string route = "api/v1/identity/confirm-email";
            var endpointUri = new Uri(string.Concat($"{origin}/", route));

            string verificationUri = QueryHelpers.AddQueryString(endpointUri.ToString(), "UserId", user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "Code", code);

            return verificationUri;
        }

        private async Task AssignDefaultRoleAndGroupsAsync(AppUser user, string source, CancellationToken cancellationToken)
        {
            await _userManager.AddToRoleAsync(user, RoleConstants.Basic);

            var query = await _gruopRepository.GetAllAsQuerable();

            var defualtGroup = query.Where(x => x.IsDefault && !x.IsDeleted).ToList();

            foreach (var group in defualtGroup)
            {
                _userGroupRepository.Add(UserGroup.Create(user.Id, group.Id));
            }
            if (defualtGroup.Count > 0)
                await _gruopRepository.SaveChangesAsync();
        }

        private async Task<AppUser> CreateUserWithPasswordAsync(string firstName, string lastName, string email, string userName, string password, string phoneNumber)
        {
            var findUserByEmail = await _userManager.FindByEmailAsync(email);
            if (findUserByEmail is not null)
                throw new CustomException("email is already exist");
            var findUserByName = await _userManager.FindByNameAsync(userName);

            //if (findUserByName is not null)
            //    throw new CustomException("userName is already exist");

            var user = new AppUser
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                UserName = userName,

            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                {

                    throw new CustomException(err.Description.ToString());
                }
            }
            return user;

        }

        private void ValidatePasswordMatch(string password, string confirmPassword)
        {
            if (password != confirmPassword)
                throw new CustomException("password not valid");
        }

        public async Task<string> ConfirmEmailAsync(string userId, string code, CancellationToken cancellationToken)
        {
            var user = _userManager.Users.Where(x => x.Id == userId && !x.EmailConfirmed).FirstOrDefault();

            _ = user ?? throw new CustomException("Can not found user");

            var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

            var result = await _userManager.ConfirmEmailAsync(user, token);

            return result.Succeeded
                       ? string.Format(CultureInfo.InvariantCulture, "Account Confirmed for E-Mail {0}. You can now use the /api/tokens endpoint to generate JWT.", user.Email)
                       : throw new CustomException(string.Format(CultureInfo.InvariantCulture, "An error occurred while confirming {0}", user.Email));

        }

        public async Task<string> ConfirmPhoneNumberAsync(string userId, string code)
        {
            var user = await _userManager.Users
            .Where(u => u.Id == userId && !u.PhoneNumberConfirmed)
             .FirstOrDefaultAsync();

            _ = user ?? throw new CustomException("An error occurred while confirming phone number.");

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ChangePhoneNumberAsync(user, user.PhoneNumber!, code);

            return result.Succeeded
                ? string.Format(CultureInfo.InvariantCulture, "Phone number {0} confirmed successfully.", user.PhoneNumber)
                : throw new CustomException(string.Format(CultureInfo.InvariantCulture, "An error occurred while confirming phone number {0}", user.PhoneNumber));

        }

        public async Task<string> GetOrCreateFromPrincipalAsync(ClaimsPrincipal principal)
        {
            var email = ExtractEmailFromPrincipal(principal);


            var finduser = await _userManager.FindByEmailAsync(email);

            if (finduser != null)
                return finduser.Id;
            var user = await CreateUserFromPrincipalAsync(principal, email);
            await AssignDefaultRoleAndGroupsAsync(user, "ExternalAuth");
            await PublishUserRegisteredAsync(user, "Identity.ExternalAuth");
            return user.Id;

        }

        private async Task PublishUserRegisteredAsync(AppUser user, string sourse)
        {
            var integrationevent = new UserRegisteredIntegrationEvent
            (
                Guid.NewGuid(),
                sourse,
                user.Id,
               user.Email ?? string.Empty,
               user.FirstName ?? string.Empty,
               user.LastName ?? string.Empty
            );

            await _publish.Publish(integrationevent);

        }

        private async Task AssignDefaultRoleAndGroupsAsync(AppUser user, string v)
        {
            var result = await _userManager.AddToRoleAsync(user, v);

            if (!result.Succeeded)
                throw new CustomException("Can not assign role to user");

        }

        private async Task<AppUser> CreateUserFromPrincipalAsync(ClaimsPrincipal principal, string email)
        {
            var (firstName, lastName, userName) = ExtractUserInfoFromPrincipal(principal, email);


            userName = await EnsureUniqueUserNameAsync(userName);

            var user = new AppUser
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                UserName = userName,
                IsActive = true,
                EmailConfirmed = true,
                PhoneNumberConfirmed = false,

            };

            var result = await _userManager.CreateAsync(user);

            if (!result.Succeeded)
            {
                var error = result.Errors.Select(x => x.Description).FirstOrDefault();

                throw new CustomException($"Failed to create user from Exteral principle: {error}");

            }

            return user;
        }

        private (string firstName, string lastName, string userName) ExtractUserInfoFromPrincipal(ClaimsPrincipal principal, string email)
        {
            var firstName = principal.FindFirstValue(ClaimTypes.GivenName)
                       ?? principal.FindFirstValue("given_name")
                       ?? string.Empty;

            var lastName = principal.FindFirstValue(ClaimTypes.Surname)
                ?? principal.FindFirstValue("family_name")
                ?? string.Empty;

            var userName = principal.FindFirstValue(ClaimTypes.Name)
                ?? principal.FindFirstValue("preferred_username")
                ?? email.Split('@')[0];

            return (firstName, lastName, userName);
        }

        private async Task<string> EnsureUniqueUserNameAsync(string userName)
        {
            var user = _userManager.FindByNameAsync(userName);
            if (user is not null)
                return $"{userName}_{Guid.NewGuid():N}"[..20];
            return userName;
        }

        private static string ExtractEmailFromPrincipal(ClaimsPrincipal principal)
        {
            return principal.FindFirstValue(ClaimTypes.Email)
            ?? principal.FindFirstValue("email")
            ?? throw new CustomException("Email claim is required for external authentication.");


        }
        private static string BuildConfirmationEmailHtml(string userName, string confirmationUrl)
        {
            return $"""
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset="utf-8">
                <meta name="viewport" content="width=device-width, initial-scale=1.0">
                <title>Confirm Your Email</title>
            </head>
            <body style="margin: 0; padding: 0; font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, 'Helvetica Neue', Arial, sans-serif; background-color: #f8fafc;">
                <table role="presentation" style="width: 100%; border-collapse: collapse;">
                    <tr>
                        <td align="center" style="padding: 40px 0;">
                            <table role="presentation" style="width: 100%; max-width: 600px; border-collapse: collapse; background-color: #ffffff; border-radius: 8px; box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);">
                                <tr>
                                    <td style="padding: 40px 40px 30px 40px; text-align: center; background-color: #2563eb; border-radius: 8px 8px 0 0;">
                                        <h1 style="margin: 0; color: #ffffff; font-size: 24px; font-weight: 600;">Confirm Your Email Address</h1>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding: 40px;">
                                        <p style="margin: 0 0 20px 0; color: #334155; font-size: 16px; line-height: 1.6;">
                                            Hi {System.Net.WebUtility.HtmlEncode(userName)},
                                        </p>
                                        <p style="margin: 0 0 20px 0; color: #334155; font-size: 16px; line-height: 1.6;">
                                            Thank you for registering! Please confirm your email address by clicking the button below:
                                        </p>
                                        <table role="presentation" style="width: 100%; border-collapse: collapse;">
                                            <tr>
                                                <td align="center" style="padding: 30px 0;">
                                                    <a href="{System.Net.WebUtility.HtmlEncode(confirmationUrl)}" style="display: inline-block; padding: 14px 32px; background-color: #2563eb; color: #ffffff; text-decoration: none; font-size: 16px; font-weight: 600; border-radius: 6px;">
                                                        Confirm Email Address
                                                    </a>
                                                </td>
                                            </tr>
                                        </table>
                                        <p style="margin: 0 0 20px 0; color: #64748b; font-size: 14px; line-height: 1.6;">
                                            If the button doesn't work, copy and paste this link into your browser:
                                        </p>
                                        <p style="margin: 0 0 20px 0; color: #2563eb; font-size: 14px; line-height: 1.6; word-break: break-all;">
                                            {System.Net.WebUtility.HtmlEncode(confirmationUrl)}
                                        </p>
                                        <p style="margin: 30px 0 0 0; color: #64748b; font-size: 14px; line-height: 1.6;">
                                            If you didn't create an account, you can safely ignore this email.
                                        </p>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding: 20px 40px; background-color: #f1f5f9; border-radius: 0 0 8px 8px; text-align: center;">
                                        <p style="margin: 0; color: #94a3b8; font-size: 12px;">
                                            This is an automated message. Please do not reply to this email.
                                        </p>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </body>
            </html>
            """;
        }
    }
}
