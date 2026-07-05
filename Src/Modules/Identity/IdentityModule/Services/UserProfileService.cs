using IdentityModule.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Module.Identity.Contract.Dtos;
using Module.Identity.Contract.Services;
using SendGrid.Helpers.Errors.Model;
using Shared.Contract.Exeption;
using Shared.DDD;
using Shared.Storage;
using Shared.Storage.Services;
using Shared.Web.Origin;

namespace IdentityModule.Services
{
    internal class UserProfileService(
        UserManager<AppUser> _userManager,
        SignInManager<AppUser> _signInManager,
        IStorageService _storageService,
        IOptions<OriginOptions> originOptions,
        IHttpContextAccessor httpContextAccessor

        ) : IUserProfileService
    {
        private readonly Uri? _originUrl = originOptions.Value.OriginUrl;
        public async Task<bool> ExistsWithEmailAsync(string email, string? exceptId = null)
        {

            return await _userManager.FindByEmailAsync(email.Normalize()) is AppUser user && user.Id != exceptId;
        }

        public async Task<bool> ExistsWithNameAsync(string name)
        {

            return await _userManager.FindByNameAsync(name) is not null;
        }

        public async Task<bool> ExistsWithPhoneNumberAsync(string phoneNumber, string? exceptId = null)
        {

            return await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber) is AppUser user && user.Id != exceptId;
        }
        public async Task<UserDto> GetAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                throw new NotFoundException("user not found");

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                FirstName = user.FirstName.name,
                LastName = user.LastName.name,
                ImageUrl = ResolveImageUrl(user.ImageUrl),
                IsActive = user.IsActive

            };

        }

        private string? ResolveImageUrl(Uri? imageUrl)
        {
            if (imageUrl is null)
            {
                return null;
            }

            // Absolute URLs (e.g., S3) pass through unchanged.
            if (imageUrl.IsAbsoluteUri)
            {
                return imageUrl.ToString();
            }

            // For relative paths from local storage, prefix with the API origin and wwwroot.
            if (_originUrl is null)
            {
                var request = httpContextAccessor.HttpContext?.Request;
                if (request is not null && !string.IsNullOrWhiteSpace(request.Scheme) && request.Host.HasValue)
                {
                    var baseUri = $"{request.Scheme}://{request.Host.Value}{request.PathBase}";
                    var relativePath = imageUrl.ToString().TrimStart('/');
                    return $"{baseUri.TrimEnd('/')}/{relativePath}";
                }

                return imageUrl.ToString();
            }

            var originRelativePath = imageUrl.ToString().TrimStart('/');
            return $"{_originUrl.AbsoluteUri.TrimEnd('/')}/{originRelativePath}";
        }

        public async Task<int> GetCountAsync(CancellationToken cancellationToken)
        {
            return await _userManager.Users.CountAsync();
        }

        public async Task<List<UserDto>> GetListAsync(CancellationToken cancellationToken)
        {
            var users = await _userManager.Users.AsNoTracking().ToListAsync();

            var userDto = new List<UserDto>(users.Count);

            foreach (var user in users)
            {
                userDto.Add(new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    FirstName = user.FirstName.name,
                    LastName = user.LastName.name,
                    ImageUrl = ResolveImageUrl(user.ImageUrl),
                    IsActive = user.IsActive

                });
            }

            return userDto;
        }

        public async Task UpdateAsync(string userId, string firstName, string lastName, string phoneNumber, FileUploadRequest image, bool deleteCurrentImage)
        {
            var user = await _userManager.FindByIdAsync(userId);

            _ = user ?? throw new CustomException("user not found");

            var imageUri = user.ImageUrl ?? null;

            if (image.Data != null && deleteCurrentImage)
            {
                var ImageUrl = await _storageService.UploadAsync<AppUser>(image, FileType.Image);

                user.ImageUrl = new Uri(ImageUrl, UriKind.RelativeOrAbsolute);

                if (imageUri != null && deleteCurrentImage)
                {
                    await _storageService.RemoveAsync(imageUri.ToString());
                }

            }

            user.FirstName = new Name(firstName);
            user.LastName = new Name(lastName);

            var curentPhoneNumber = await _userManager.GetPhoneNumberAsync(user);

            if (curentPhoneNumber != phoneNumber)
            {
                var phoneResult = await _userManager.SetPhoneNumberAsync(user, phoneNumber);

                if (!phoneResult.Succeeded)
                    throw new CustomException(phoneResult.Errors.FirstOrDefault().Description);
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                throw new CustomException("somthing wrong while update user profile ");
            }

        }
    }
}
