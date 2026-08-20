using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Module.Tax.Data;

public sealed class TaxDbContextFactory : IDesignTimeDbContextFactory<TaxDbContext>
{
    public TaxDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TaxDbContext>();
        var migrationsAssembly = typeof(TaxDbContext).Assembly.GetName().Name ?? typeof(TaxDbContext).Namespace ?? "Module.Tax";
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__defualt")
            ?? "Host=localhost;Port=5432;Database=Eshop_Tax;Username=postgres;Password=postgres";

        optionsBuilder.UseNpgsql(connectionString, options => options.MigrationsAssembly(migrationsAssembly));
        return new TaxDbContext(optionsBuilder.Options);
    }
}
