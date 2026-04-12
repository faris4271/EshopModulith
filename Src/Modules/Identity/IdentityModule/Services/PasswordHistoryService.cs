using IdentityModule.Data;
using IdentityModule.Domain;
using MassTransit.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Module.Identity.Contract.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityModule.Services
{
    internal class PasswordHistoryService : IPasswordHistoryService
    {
        private readonly IdentityDbContext _identityDb;

        private readonly UserManager<AppUser> _userManager;

        private readonly PasswordPolicyOptions _passwordPolicyOptions;
        public PasswordHistoryService(IdentityDbContext identityDb, 
            UserManager<AppUser> userManager,IOptions<PasswordPolicyOptions> passwordPolicyOptions)
        {
            _identityDb = identityDb;
            _userManager = userManager;
            _passwordPolicyOptions = passwordPolicyOptions.Value;
        }

        public async Task CleanupOldPasswordHistoryAsync(string userId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(userId, nameof(userId));

            var count = _passwordPolicyOptions.PasswordHistoryCount;

            if(count<=0)
            {
                return;
            }

            var oldPasswordHistory=await _identityDb.passwordHistory.
                Where(x=>x.UserId==userId).
                OrderByDescending(x=>x.CreatedAt).
                ToListAsync(cancellationToken);

            if (oldPasswordHistory != null)
            {
                var passwordHistoryToRemove=oldPasswordHistory.Skip(count).ToList();
                if (passwordHistoryToRemove.Any())
                {
                    _identityDb.passwordHistory.RemoveRange(passwordHistoryToRemove);
                    await _identityDb.SaveChangesAsync(cancellationToken);
                }
            }
        }

        public async Task<bool> IsPasswordInHistoryAsync(string userId, string newPassword, CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrEmpty(userId, nameof(userId));
            ArgumentException.ThrowIfNullOrEmpty(newPassword, nameof(newPassword));

            var count=_passwordPolicyOptions.PasswordHistoryCount;

            if (count == 0)
            {
                return false;
            }

            var user =await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                return false;
            }

            var passwordhistory=_identityDb.Set<PasswordHistory>().
                Where(x=>x.UserId==userId).
                OrderByDescending(x=>x.CreatedAt).
                Select(x=>x.PasswordHash).
                Take(count).ToList();

            foreach(var passwordHash in passwordhistory)
            {
                var passwordHasher = _userManager.PasswordHasher;

                var result = passwordHasher.VerifyHashedPassword(user, passwordHash, newPassword);

                if(result==PasswordVerificationResult.Success || result== PasswordVerificationResult.SuccessRehashNeeded)
                {
                    return true;
                }
            }
            return false;

        }

        public async Task SavePasswordHistoryAsync(string userId, CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrEmpty(userId, nameof(userId));

            var user = await _userManager.FindByIdAsync(userId);

            if (user is null||  string.IsNullOrEmpty(user.PasswordHash))
            {
                return;
            }

            var passwordHistory = PasswordHistory.Create(userId, user.PasswordHash);

            await _identityDb.passwordHistory.AddAsync(passwordHistory, cancellationToken);

            await _identityDb.SaveChangesAsync(cancellationToken);

            await CleanupOldPasswordHistoryAsync(userId, cancellationToken);


        }
    }
}
