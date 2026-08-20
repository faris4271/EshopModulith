using Eshop.Module.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EShop.Module.Core.Data;

public sealed class CoreDbContextFactory : IDesignTimeDbContextFactory<CoreDbContext>
{
    public CoreDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CoreDbContext>();
        var migrationsAssembly = typeof(CoreDbContext).Assembly.GetName().Name ?? typeof(CoreDbContext).Namespace ?? "EShop.Module.Core";
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__defualt")
            ?? "Host=localhost;Port=5432;Database=Eshop_Core;Username=postgres;Password=postgres";

        optionsBuilder.UseNpgsql(connectionString, options => options.MigrationsAssembly(migrationsAssembly));
        return new CoreDbContext(optionsBuilder.Options);
    }
}
