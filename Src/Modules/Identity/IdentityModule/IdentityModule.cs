using FSH.Framework.Storage;
using Hangfire;
using Hangfire.Common;
using IdentityModule.Data;
using IdentityModule.Domain;
using IdentityModule.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Module.Identity.Contract.Services;
using Shared.Contract.Context;

namespace IdentityModule;

public static class IdentityModule 
{
    public static IServiceCollection AddIdentityModule(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddDbContext<IdentityDbContext>(op =>
        {
            op.UseNpgsql(configuration.GetConnectionString("IdentityDb"));

        });
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<Module.Identity.Contract.Services.ICurrentUser>(sp => sp.GetRequiredService<ICurrentUserService>());
        services.AddScoped<Module.Identity.Contract.Services.ICurrentUserInitializer>(sp => sp.GetRequiredService<ICurrentUserService>());
        services.AddScoped<IRequestContextService, RequestContextService>();
        services.AddScoped<IRequestContext>(sp => sp.GetRequiredService<IRequestContextService>());
        services.AddScoped<ITokenServic, TokenServic>();

        // User services - focused single-responsibility services
        services.AddTransient<IUserRegistrationService, UserRegistrationService>();
        services.AddTransient<IUserRoleService, UserRoleService>();
        services.AddTransient<IUserPasswordService, UserPasswordService>();
        services.AddTransient<IUserPermissionService, UserPermissionService>();

        // Facade for backward compatibility
       

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

        return services;


    }


}
