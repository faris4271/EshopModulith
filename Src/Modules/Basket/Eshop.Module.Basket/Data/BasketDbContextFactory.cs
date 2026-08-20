using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Eshop.Module.Basket.Data;

public sealed class BasketDbContextFactory : IDesignTimeDbContextFactory<BasketDbContext>
{
    public BasketDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BasketDbContext>();
        var migrationsAssembly = typeof(BasketDbContext).Assembly.GetName().Name ?? typeof(BasketDbContext).Namespace ?? "Eshop.Module.Basket";
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__defualt")
            ?? "Host=localhost;Port=5432;Database=Eshop_Basket;Username=postgres;Password=postgres";

        optionsBuilder.UseNpgsql(connectionString, options =>
        {
            options.MigrationsAssembly(migrationsAssembly);
        });

        return new BasketDbContext(optionsBuilder.Options);
    }
}
