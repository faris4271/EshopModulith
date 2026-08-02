using Microsoft.EntityFrameworkCore;

public sealed class CheckoutDbContext(DbContextOptions<CheckoutDbContext> options) : DbContext(options)
{
    public DbSet<Checkout> Checkouts => Set<Checkout>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("checkout");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CheckoutDbContext).Assembly);
    }
}