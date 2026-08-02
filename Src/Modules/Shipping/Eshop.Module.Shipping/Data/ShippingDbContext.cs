using Microsoft.EntityFrameworkCore;

namespace Eshop.Module.Shipping.Data;

public sealed class ShippingDbContext(DbContextOptions<ShippingDbContext> options) : DbContext(options)
{


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("shipping");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShippingDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}