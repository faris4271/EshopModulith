using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Module.Inventory.Data;

public sealed class InventoryDbContextFactory : IDesignTimeDbContextFactory<InventoryDbContext>
{
    public InventoryDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<InventoryDbContext>();
        var migrationsAssembly = typeof(InventoryDbContext).Assembly.GetName().Name ?? typeof(InventoryDbContext).Namespace ?? "Module.Inventory";
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__defualt")
            ?? "Host=localhost;Port=5432;Database=Eshop_Inventory;Username=postgres;Password=postgres";

        optionsBuilder.UseNpgsql(connectionString, options => options.MigrationsAssembly(migrationsAssembly));
        return new InventoryDbContext(optionsBuilder.Options);
    }
}
