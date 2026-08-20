using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MultiTenancyModule.Data;

public sealed class TenantDbContextFactory : IDesignTimeDbContextFactory<TenantDbContext>
{
    public TenantDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TenantDbContext>();
        var migrationsAssembly = typeof(TenantDbContext).Assembly.GetName().Name ?? typeof(TenantDbContext).Namespace ?? "MultiTenancyModule";
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__defualt")
            ?? "Host=localhost;Database=multitenancy_design;Username=postgres;Password=postgres";

        optionsBuilder.UseNpgsql(connectionString, options =>
        {
            options.MigrationsAssembly(migrationsAssembly);
        });

        return new TenantDbContext(optionsBuilder.Options);
    }
}
