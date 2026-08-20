using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Catalog.Data;

public sealed class CatalogDbContextFactory : IDesignTimeDbContextFactory<CatalogDbContext>
{
    public CatalogDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CatalogDbContext>();
        var migrationsAssembly = typeof(CatalogDbContext).Assembly.GetName().Name ?? typeof(CatalogDbContext).Namespace ?? "Catalog";
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__defualt")
            ?? "Host=localhost;Port=5432;Database=Eshop_Catalog;Username=postgres;Password=postgres";

        optionsBuilder.UseNpgsql(connectionString, options => options.MigrationsAssembly(migrationsAssembly));
        return new CatalogDbContext(optionsBuilder.Options);
    }
}
