using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace IdentityModule.Data;

public sealed class IdentityDbContextFactory : IDesignTimeDbContextFactory<IdentityDbContext>
{
    public IdentityDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<IdentityDbContext>();
        var migrationsAssembly = typeof(IdentityDbContext).Assembly.GetName().Name ?? typeof(IdentityDbContext).Namespace ?? "IdentityModule";
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__defualt")
            ?? "Host=localhost;Port=5432;Database=Eshop_Identity;Username=postgres;Password=postgres";

        optionsBuilder.UseNpgsql(connectionString, options => options.MigrationsAssembly(migrationsAssembly));
        return new IdentityDbContext(optionsBuilder.Options);
    }
}
