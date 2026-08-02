using Microsoft.EntityFrameworkCore;

namespace Eshop.Module.Checkout.Data;

public sealed class CheckoutDbContext(DbContextOptions<CheckoutDbContext> options) : DbContext(options)
{
    public DbSet<Entities.Checkout> Checkouts => Set<Entities.Checkout>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("checkout");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CheckoutDbContext).Assembly);
    }


}
