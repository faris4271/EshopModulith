using IdentityModule.Domain;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Module.Identity.Contract.Dtos;
using Module.Identity.Contract.Services;
using Shared.Contract.Exeption;
using Shared.Contract.Identity;
using Shared.GlobalConfig;
using System.Security.Claims;

namespace IdentityModule.Services;

internal sealed class CurrentUserService : ICurrentUserService
{
    private readonly HttpContext _httpContext;
    private readonly UserManager<AppUser> _userManager;
    private const string UserGuidCookiesName = "UserGuid";
    private readonly IConfiguration _configuration;
    public CurrentUserService(IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager, IConfiguration configuration)
    {
        _httpContext = httpContextAccessor.HttpContext;
        _userManager = userManager;
        _configuration = configuration;
    }
    private ClaimsPrincipal? _user;

    private UserDto _currentUser;

    public string? Name => _user?.Identity?.Name;

    private Guid _userId = Guid.Empty;

    public Guid GetUserId()
    {
        return IsAuthenticated()
            ? Guid.Parse(_user?.GetUserId() ?? Guid.Empty.ToString())
            : _userId;
    }

    public async Task<UserDto> GetCurrentUser()
    {
        if (_currentUser != null)
            return _currentUser;

        var contextUser = _user;

        var user = await _userManager.GetUserAsync(contextUser);

        if (user != null)
        {
            return _currentUser = user.Adapt<UserDto>();

        }

        var UserGuid = GetUserGuidFromCookies();


        if (UserGuid.HasValue)
        {
            user = await _userManager.FindByIdAsync(UserGuid.Value.ToString());

        }

        var userRole = await _userManager.GetRolesAsync(user);

        if (user != null && userRole.Count == 1 && userRole[0] == "Guest")
        {
            return _currentUser = user.Adapt<UserDto>();
        }

        UserGuid = Guid.NewGuid();
        var dummyEmail = string.Format("{0}@guest.simplcommerce.com", UserGuid);
        user = new AppUser
        {
            FirstName = "Guest",
            Id = UserGuid.Value.ToString(),
            Email = dummyEmail,
            UserName = dummyEmail,
            Culture = _configuration.GetValue<string>("Global.DefaultCultureUI") ?? GlobalConfiguration.DefaultCulture
        };
        var abc = await _userManager.CreateAsync(user, "1qazZAQ!");
        await _userManager.AddToRoleAsync(user, "guest");
        SetUserGuidCookies();
        return _currentUser = user.Adapt<UserDto>();

    }
    private void SetUserGuidCookies()
    {
        _httpContext.Response.Cookies.Append(UserGuidCookiesName, _currentUser.Id, new CookieOptions
        {
            Expires = DateTime.UtcNow.AddYears(5),
            HttpOnly = true,
            IsEssential = true,
            SameSite = SameSiteMode.Strict
        });
    }
    private Guid? GetUserGuidFromCookies()
    {
        if (_httpContext.Request.Cookies.ContainsKey(UserGuidCookiesName))
        {
            return Guid.Parse(_httpContext.Request.Cookies[UserGuidCookiesName]);
        }

        return null;
    }

    public string? GetUserEmail() =>
        IsAuthenticated()
            ? _user!.GetEmail()
            : string.Empty;

    public bool IsAuthenticated()
    {

        return (_user?.Identity?.IsAuthenticated ?? _httpContext.User?.Identity?.IsAuthenticated) is true;
    }

    public bool IsInRole(string role) =>
        _user?.IsInRole(role) is true;

    public IEnumerable<Claim>? GetUserClaims() =>
        _user?.Claims;

    public string? GetTenant() =>
        IsAuthenticated() ? _user?.GetTenant() : string.Empty;

    public void SetCurrentUser(ClaimsPrincipal user)
    {
        if (_user != null)
        {
            throw new CustomException("Method reserved for in-scope initialization");
        }

        _user = user;
    }

    public void SetCurrentUserId(string userId)
    {
        if (_userId != Guid.Empty)
        {
            throw new CustomException("Method reserved for in-scope initialization");
        }

        if (!string.IsNullOrEmpty(userId))
        {
            _userId = Guid.Parse(userId);
        }
    }
}