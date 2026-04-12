using IdentityModule.Data;
using IdentityModule.Domain;
using Microsoft.AspNetCore.Identity;
using Module.Identity.Contract.Dtos;
using Module.Identity.Contract.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityModule.Services
{
    internal class PasswordExpiryService : IPasswordExpiryService
    {
        private readonly UserManager<AppUser> _userManager;

        private readonly PasswordPolicyOptions _passwordPolicyOptions;

        public PasswordExpiryService(UserManager<AppUser> userManager, PasswordPolicyOptions passwordPolicyOptions)
        {
            _userManager = userManager;
            _passwordPolicyOptions = passwordPolicyOptions;
        }

        public async Task<int> GetDaysUntilExpiryAsync(string userId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(userId, nameof(userId));

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) 
                return int.MaxValue;

            return GetDaysUntilExpiry(user);

        }

        public async Task<PasswordExpiryStatusDto> GetPasswordExpiryStatusAsync(string userId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(userId, nameof(userId));

            var user =await _userManager.FindByIdAsync(userId);

            if (user == null) 
                return new PasswordExpiryStatusDto
                {
                    IsExpired = false,
                    IsExpiringWithinWarningPeriod = false,
                    DaysUntilExpiry = int.MaxValue,
                    ExpiryDate = null
                };
            return GetPasswordExpiryStatus(user);
        }

        public async Task<bool> IsPasswordExpiredAsync(string userId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(userId, nameof(userId));

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return false;

            return IsPasswordExpired(user);
        }

        public async Task<bool> IsPasswordExpiringWithinWarningPeriodAsync(string userId, CancellationToken cancellationToken = default)
        {
            if(userId == null)
                return false;

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return false;

            return IsPasswordExpiringWithinWarningPeriod(user);
        }

        public async Task UpdateLastPasswordChangeDateAsync(string userId, CancellationToken cancellationToken = default)
        {
            var user =await _userManager.FindByIdAsync(userId);

            if(user is not  null)
            {
                user.LastPasswordChangeDate = DateTime.UtcNow;
                await _userManager.UpdateAsync(user);
            }
        }
        private int GetDaysUntilExpiry(AppUser user)
        {
            if (!_passwordPolicyOptions.EnforcePasswordExpiry)
            {
                return int.MaxValue;
            }

            var expirationDate = user.LastPasswordChangeDate.AddDays(_passwordPolicyOptions.PasswordExpiryDays);
            var dayUntilExpired=(int) (expirationDate - DateTime.UtcNow ).TotalDays;
            return dayUntilExpired;
        }

        private bool IsPasswordExpired(AppUser user)
        {
            if (_passwordPolicyOptions.EnforcePasswordExpiry) 
                return false;

            var passwordExpirationDate = user.LastPasswordChangeDate.AddDays(_passwordPolicyOptions.PasswordExpiryDays);

            return DateTime.UtcNow > passwordExpirationDate;

        }
        private bool IsPasswordExpiringWithinWarningPeriod(AppUser user)
        {
            if (!_passwordPolicyOptions.EnforcePasswordExpiry)
            {
                return false;
            }

            var daysUntilExpiry = GetDaysUntilExpiry(user);
            return daysUntilExpiry >= 0 && daysUntilExpiry <= _passwordPolicyOptions.PasswordExpiryWarningDays;
        }
        private PasswordExpiryStatusDto GetPasswordExpiryStatus(AppUser user)
        {
            var expiryDate = user.LastPasswordChangeDate.AddDays(_passwordPolicyOptions.PasswordExpiryDays);
            var daysUntilExpiry = GetDaysUntilExpiry(user);
            var isExpired = IsPasswordExpired(user);
            var isExpiringWithinWarningPeriod = IsPasswordExpiringWithinWarningPeriod(user);

            return new PasswordExpiryStatusDto
            {
                IsExpired = isExpired,
                IsExpiringWithinWarningPeriod = isExpiringWithinWarningPeriod,
                DaysUntilExpiry = daysUntilExpiry,
                ExpiryDate = _passwordPolicyOptions.EnforcePasswordExpiry ? expiryDate : null
            };
        }
    }
}
