using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Eshop.Module.Localization.Data;

public sealed class LocalizationDbContextFactory : IDesignTimeDbContextFactory<LocalizationDbContext>
{
    public LocalizationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<LocalizationDbContext>();
        var migrationsAssembly = typeof(LocalizationDbContext).Assembly.GetName().Name ?? typeof(LocalizationDbContext).Namespace ?? "Eshop.Module.Localization";
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__defualt")
            ?? "Host=localhost;Port=5432;Database=Eshop_Localization;Username=postgres;Password=postgres";

        optionsBuilder.UseNpgsql(connectionString, options => options.MigrationsAssembly(migrationsAssembly));
        return new LocalizationDbContext(optionsBuilder.Options);
    }
}
