using FSH.Framework.Storage;
using IdentityModule.Authorization.Jwt;
using IdentityModule.Data;
using IdentityModule.Domain;
using IdentityModule.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Module.Identity.Contract.Services;
using Shared.Contract.Context;
using Shared.Data;
using Shared.Eventing;
using Shared.Persistence;
using UAParser;

namespace IdentityModule;

public static class IdentityModule
{
    public static IServiceCollection AddIdentityModule(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddDbContext<IdentityDbContext>(op =>
        {
            op.UseNpgsql(configuration.GetConnectionString("defualt"));

        });
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<ICurrentUser>(sp => sp.GetRequiredService<ICurrentUserService>());
        services.AddScoped<ICurrentUserInitializer>(sp => sp.GetRequiredService<ICurrentUserService>());
        services.AddScoped<IRequestContextService, RequestContextService>();
        services.AddScoped<IRequestContext>(sp => sp.GetRequiredService<IRequestContextService>());
        services.AddScoped<ITokenServic, TokenServic>();

        // User services - focused single-responsibility services
        services.AddTransient<IUserRegistrationService, UserRegistrationService>();
        services.AddTransient<IUserRoleService, UserRoleService>();
        services.AddTransient<IUserPasswordService, UserPasswordService>();
        services.AddTransient<IUserPermissionService, UserPermissionService>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IUserProfileService, UserProfileService>();
        services.AddTransient<IUserStatusService, UserStatusService>();
        services.AddTransient<ISessionService, SessionService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddEventingForDbContext<IdentityDbContext>();
        // Facade for backward compatibility

        services.AddScoped<IDbInitializer, IdentityDbInitializer>();

        services.AddHeroLocalFileStorage();
        services.AddScoped<IIdentityService, IdentityService>();

        // Configure password policy options
        services.Configure<PasswordPolicyOptions>(configuration.GetSection("PasswordPolicy"));
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<PasswordPolicyOptions>>().Value);

        // Register password history service
        services.AddScoped<IPasswordHistoryService, PasswordHistoryService>();

        // Register password expiry service
        services.AddScoped<IPasswordExpiryService, PasswordExpiryService>();


        // Register group role service for group-derived permissions
        services.AddScoped<IGroupRoleService, GroupRoleService>();

        services.AddIdentity<AppUser, Role>(options =>
        {
            options.Password.RequiredLength = IdentityModuleConstants.PasswordLength;
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.User.RequireUniqueEmail = true;
        }).AddEntityFrameworkStores<IdentityDbContext>()
          .AddDefaultTokenProviders(); ;


        //metrics
        services.AddSingleton<IdentityMetrics>();


        services.ConfigureJwtAuth(configuration);

        services.AddSingleton<Parser>(sp => Parser.GetDefault());


        return services;


    }
    public static async Task<IApplicationBuilder> UseIdentity(this IApplicationBuilder app)
    {

        app.UseMigration<IdentityDbContext>();


        return app;
    }


}



