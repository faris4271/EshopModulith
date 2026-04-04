using System.Security.Claims;

namespace Module.Identity.Contract.Services
{
    public interface IUserRegistrationService
    {
        Task<string> RegisterAsync(
      string firstName,
      string lastName,
      string email,
      string userName,
      string password,
      string confirmPassword,
      string phoneNumber,
      string origin,
      CancellationToken cancellationToken);

        /// <summary>
        /// Gets or creates a user from an external authentication principal.
        /// </summary>
        Task<string> GetOrCreateFromPrincipalAsync(ClaimsPrincipal principal);

        /// <summary>
        /// Confirms a user's email address.
        /// </summary>
        Task<string> ConfirmEmailAsync(string userId, string code, string tenant, CancellationToken cancellationToken);

        /// <summary>
        /// Confirms a user's phone number.
        /// </summary>
        Task<string> ConfirmPhoneNumberAsync(string userId, string code);
    }
}
