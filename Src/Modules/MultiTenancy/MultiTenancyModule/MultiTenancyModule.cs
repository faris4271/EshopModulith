using Finbuckle.MultiTenant.EntityFrameworkCore.Extensions;
using Finbuckle.MultiTenant.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MultiTenancyModule.Authorization;
using MultiTenancyModule.Contract.Services;
using MultiTenancyModule.Data;
using MultiTenancyModule.Provisioning;
using MultiTenancyModule.Services;
using Shared.Data;
using Shared.Eventing;
using Shared.Identity;
using Shared.Multitenancy;
using Shared.Persistence;

namespace MultiTenancyModule;

public static class MultiTenancyModule
{
    public static IServiceCollection AddMultiTenancyModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHeroDbContext<TenantDbContext>();

        services.AddDbContext<TenantDbContext>(op =>
        {
            op.UseNpgsql(configuration.GetConnectionString("defualt"), b => b.MigrationsAssembly(typeof(TenantDbContext).Assembly.FullName));
        });

        // Register permissions
        PermissionConstants.Register(MultiTenancyPermissions.All);

        // Replace 'TenantProvisioningService' with your actual implementation class name
        services.AddMultiTenant<AppTenantInfo>()
            .WithEFCoreStore<TenantDbContext, AppTenantInfo>();

        services.AddScoped<ITenantProvisioningService, TenantProvisioningService>();
        services.AddScoped<ITenantService, TenantService>();
        services.AddScoped<ITenantThemeService, TenantThemeService>();
        services.AddScoped<ITenantSettingsService, TenantSettingsService>();
        //services.AddTransient<IConnectionStringValidator, ConnectionStringValidator>();
        services.AddTransient<TenantProvisioningJob>();
        //services.AddTransient<TenantExpiryScanJob>();


        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(MultiTenancyModule).Assembly);
        });

        services.AddEventingForDbContext<TenantDbContext>();
        services.AddIntegrationEventHandlers(typeof(MultiTenancyModule).Assembly);

        return services;
    }


    public static IApplicationBuilder UseMultiTenancy(this IApplicationBuilder app)
    {
        app.UseMigration<TenantDbContext>();
        return app;
    }
}